using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.ApiLauncher {

    /// <summary>
    /// This class is a testing tool -- used to launch 
    /// one or more Api dependencies when in Development.  
    /// </summary>
    public class Launcher : IDisposable {

        //private variables for various injected objects
        private static ILogger _logger;
        private readonly DotNetProcessTerminator _terminator;
        private readonly IConfiguration _config;

        //variables for configuration data
        private readonly string _defaultRepoDir;
        private readonly int _startingPort;

        public const int MAX_TIMEOUT_IN_SECONDS = 30;

        /// <summary>
        /// Instantiates a new Launcher object
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logger"></param>
        /// <param name="terminator"></param>
        public Launcher(IConfiguration config, ILogger<Launcher> logger, DotNetProcessTerminator terminator) {
            _config = config;
            _defaultRepoDir = config["DefaultRepoDirectory"];
            _startingPort = int.Parse(config["StartingPort"]);
            _logger = logger;
            _terminator = terminator;
        }

        //holds references to all launched APIs
        public Dictionary<string,Api> LaunchedApis = new Dictionary<string,Api>();


        /// <summary>
        /// Starts all APIs referenced in configuration
        /// </summary>
        /// <param name="config">Configuration holding data for APIs</param>
        public void StartApis(Dictionary<string,Api> needApis, string clientId) {

            //ignore all apis where there is a non-localhost URL 
            needApis = needApis.Where(n => n.Value.BaseAddress == null || n.Value.BaseAddress.StartsWith("http://localhost"))
                .ToDictionary(a => a.Key, a => a.Value) ;


            _logger.LogInformation("Starting Apis");

            //get a set of ports to be used by localhost for new APIs
            var ports = PortInspector.GetAvailablePorts(_startingPort, needApis.Count);

            //get the API data from the configuration
            //iterate over all API data
            int i = -1;
            foreach (var api in needApis) {
                //update the project name
                api.Value.ProjectName = api.Key;

                i++;//manually update because of dictionary iteration

                //handle requests for already-launched APIs
                var existingApi = LaunchedApis.Where(a => a.Key == api.Key).FirstOrDefault().Value;

                if (existingApi != default(Api)) {
                    //just add the API to the client list
                    if(!existingApi.Clients.Exists(e=>e==clientId))
                        existingApi.Clients.Add(clientId);
                    ports[i] = existingApi.Port;
                    continue;
                }
                //start the API
                StartApi(api.Value, ports[i], clientId);
            }

            //wait for all of the APIs to be ready for use
            ApiAwaiter.AwaitApis(ports, _logger);

            //wait for all of the APIs to have all their info
            var sw = new Stopwatch();
            while (sw.ElapsedMilliseconds < (1000 * MAX_TIMEOUT_IN_SECONDS)) {
                foreach (var n in needApis.Values) { 
                    if (!LaunchedApis.ContainsKey(n.ProjectName)) {
                        Thread.Sleep(1000);
                        continue;
                    }
                    n.Ready = true;
                }
                break;
            }
            sw.Stop();

        }


        /// <summary>
        /// Starts the API in a new thread
        /// </summary>
        /// <param name="api">The Api to start</param>
        /// <param name="port">The port to assign to the Api</param>
        /// <param name="clientId">the MQTT client id</param>
        private void StartApi(Api api, int port, string clientId) {

            _logger.LogInformation($"Starting Api: {api.ProjectName} @ {port} ");

            //handle any command-line arguments
            var commandLineArgs = "";
            if (api.CommandLineArgs != null)
                commandLineArgs = $"--{api.CommandLineArgs}";

            //update the BaseAddress
            if (api.BaseAddress == null)
                api.BaseAddress = $"http://localhost:{port}";


            //configure a background process for running dotnet,
            //ensuring that the port is set appropriately and
            //that all console output is to the same console
            var info = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = $"/c dotnet run --no-build --urls {api.BaseAddress} {commandLineArgs}",
                CreateNoWindow = true,
                WorkingDirectory = api.LocalProjectDirectory
            };

            //call the dotnet run command asynchronously
            Task.Run(() => {
                Process p = new Process {
                    StartInfo = info
                };
                try {
                    _logger.LogInformation($"trying to start {api.ProjectName} @ {port}");
                    p.Start();
                } catch (Exception ex) {
                    _logger.LogInformation($"EXCEPTION: {ex.Message}");
                }

                _logger.LogInformation($"Starting {api.ProjectName} @ {port}");

                //update the default directory
                if (_defaultRepoDir != null)
                    api.RepoDirectory = _defaultRepoDir;

                //update the full project path
                api.FullProjectPath = api.FullProjectPath ?? $"{api.LocalProjectDirectory}";

                //update the port and base address
                api.Port = port;
                api.BaseAddress = api.BaseAddress ?? $"http://localhost:{port}";

                //add the current clientId to the list of clients
                //using the API
                api.Clients.Add(clientId);

                //add the launched Api to the dictionary of running APIs
                api.Process = p;
                LaunchedApis.Add(api.ProjectName,api);

                //wait for the process to be suspended.
                p.WaitForExit();
            });
        }

        /// <summary>
        /// Stop a specific API
        /// </summary>
        public void StopApi(Api api) {
            //stop the server
            api.Process.Close();
            _logger.LogInformation($"Stopping {api.ProjectName} @ {api.Port}");

            //remove the server from the dictionary of running servers
            LaunchedApis.Remove(api.ProjectName);
        }


        /// <summary>
        /// Stops all running APIs for a given client
        /// </summary>
        public void StopApis(string clientId) {
            var ports = new List<int>();

            //get a list of the running APIs
            var projects = LaunchedApis.Keys.ToArray();

            //iterate over the list, removing the
            //current client and also stopping the
            //the API when there are no more clients
            for(int i=0; i<projects.Length; i++) {
                var api = LaunchedApis[projects[i]];

                //remove the client 
                api.Clients.Remove(clientId);

                //if there are no more clients, stop the API
                if(api.Clients.Count() == 0) {
                    ports.Add(api.Port);
                    _logger.LogInformation($"Stopping {api.ProjectName} @ {api.Port}");
                    StopApi(api);
                }
            }
            try {
                //fail-safe measure to kill any ports whose processes should be terminated
                if(ports.Count > 0)
                    _terminator.KillDotNetProcesses(ports.ToArray());
            } catch { }
        }


        /// <summary>
        /// Stops all running APIs
        /// </summary>
        public void StopApis() {
            var ports = new List<int>();

            //get a list of the running APIs
            var projects = LaunchedApis.Keys.ToArray();

            //iterate over the list, removing their
            //associated clients and also stopping the
            //the API 
            for (int i = 0; i < projects.Length; i++) {
                var api = LaunchedApis[projects[i]];
                api.Clients.Clear();
                ports.Add(api.Port);
                _logger.LogInformation($"Stopping {api.ProjectName} @ {api.Port}");
                StopApi(api);
            }
            try {
                //fail-safe measure to kill any ports whose processes should be terminated
                _terminator.KillDotNetProcesses(ports.ToArray());
            } catch { }
        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if(LaunchedApis.Count > 0)
                        StopApis(); //stop all APIs upon disposal of this class
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            Dispose(true);
        }
        #endregion

    }

    /// <summary>
    /// Provides convenience methods for constructing URLs
    /// </summary>
    public static class StringExtensions {

        /// <summary>
        /// Appends a path segment to a source path, handling duplicate
        /// forward slashes and backslashes.
        /// </summary>
        /// <param name="source">The source path (assumed to be valid)</param>
        /// <param name="stringToAppend">The path segment to append</param>
        /// <returns></returns>
        public static string AppendPath(this string source, string stringToAppend) {

            if ((source.EndsWith("\\") || source.EndsWith("/")) && (stringToAppend.StartsWith("\\") || stringToAppend.StartsWith("/")))
                return source + stringToAppend.Substring(1);
            else if (!source.EndsWith("\\") && !source.EndsWith("/") && !stringToAppend.StartsWith("\\") && !stringToAppend.StartsWith("/")) {
                if (source.Contains("\\") || stringToAppend.Contains("/"))
                    return source + "\\" + stringToAppend;
                else
                    return source + "/" + stringToAppend;
            } else
                return source + stringToAppend;

        }
    }
}
