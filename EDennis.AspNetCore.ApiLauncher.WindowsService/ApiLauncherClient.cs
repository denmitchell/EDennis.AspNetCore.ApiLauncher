using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.ApiLauncher {


    /// <summary>
    /// This class uses the Generic Host service as a client
    /// to the ApiLauncherWindowsService.
    /// </summary>
    public class ApiLauncherClient : IHostedService, IDisposable {

        //references to various injected objects
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;
        private IConfiguration _config;

        //container for response message from service 
        private Dictionary<string,Api> _haveApis = new Dictionary<string,Api>();
        //client that communicates with Windows service via MQTT protocol
        private IMqttClient _mqttClient;
        //the id of the MQTT client
        private readonly string _clientId;

        private const string SERVER_ACCEPT_TOPIC = "NeedApis";
        private const string SERVER_PUBLISH_TOPIC = "HaveApis";


        /// <summary>
        /// Constructs a new ApiLauncherClient instance with the
        /// injected Logger, Configuration, and Environment
        /// </summary>
        /// <param name="logger">ILogger implementation</param>
        /// <param name="config">IConfiguration singleton</param>
        /// <param name="env">IHostingEnvironment singleton</param>
        public ApiLauncherClient(ILogger<ApiLauncherClient> logger,
            IConfiguration config, IHostingEnvironment env) {
            _logger = logger;
            _config = config;
            _env = env;
            _clientId = env.ApplicationName + "-" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);
        }

        /// <summary>
        /// Starts the MQTT client, sends a request to start Apis to
        /// the Windows service, and updates the Configuration object
        /// with BaseAddress and Ready information for each Api.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken) {
            _logger.LogInformation("Api Launcher Windows Service Client is starting.");

            //get the current Api information from configuration
            var apis = new Dictionary<string, Api>();
            _config.GetSection("Apis").Bind(apis);

            //update ProjectName and RepoDirectory info 
            foreach (var api in apis) {
                api.Value.ProjectName = api.Key;
                if (_config["DefaultRepoDirectory"] != null)
                    api.Value.RepoDirectory = _config["DefaultRepoDirectory"];
            }

            //get the port for the ApiLauncher Windows Service
            var apiLauncherPort = int.Parse(_config["ApiLauncherPort"] ?? "1883");

            //build options for the MQTT client
            var options = new MqttClientOptionsBuilder()
                .WithClientId(_clientId)
                .WithTcpServer("localhost", apiLauncherPort) // Port is optional
                .Build();

            // Create a new MQTT client.
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            //define an event handler for a new connection
            _mqttClient.Connected += async (s, e) => {
                _logger.LogInformation("ApiLauncherWindowsServiceClient connected.");

                // Subscribe to a topic
                await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic($"HaveApis/{_clientId}").Build());

                _logger.LogInformation($"ApiLauncherWindowsServiceClient subscribed to HaveApis /{ _clientId}.");
            };

            //define an event handler for received messages
            _mqttClient.ApplicationMessageReceived += (s, e) => {

                //deserialize the message into a Dictionary<string,Api>
                var json = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                _haveApis = JToken.Parse(json).ToObject<Dictionary<string,Api>>();

                //update the configuration object with data from the received message
                foreach (var haveApi in _haveApis) {
                    _config[$"Apis:{haveApi.Value.ProjectName}:BaseAddress"] = haveApi.Value.BaseAddress ?? $"http://localhost:{haveApi.Value.Port}";
                    _config[$"Apis:{haveApi.Value.ProjectName}:ProjectName"] = haveApi.Value.ProjectName;
                    _config[$"Apis:{haveApi.Value.ProjectName}:Port"] = haveApi.Value.Port.ToString();
                    _config[$"Apis:{haveApi.Value.ProjectName}:FullProjectPath"] = haveApi.Value.LocalProjectDirectory;
                    _config[$"Apis:{haveApi.Value.ProjectName}:Ready"] = "true";
                }
            };

            //wait for a connection
            _mqttClient.ConnectAsync(options).Wait();

            //construct the message body by serializing the apis dictionary
            var needApisJson = JToken.FromObject(apis).ToString();

            //build the MQTT message
            var msg = new MqttApplicationMessageBuilder()
                .WithExactlyOnceQoS()
                .WithTopic(SERVER_ACCEPT_TOPIC + "/" + _clientId)
                .WithPayload(needApisJson)
                .Build();

            //send the message
            _mqttClient.PublishAsync(msg);

            return Task.CompletedTask;

        }


        /// <summary>
        /// Stops all APIs for which the current application is
        /// the only client currently using them.  Note that the
        /// client has to disconnect.  When the connection is 
        /// broken, the Windows Service knows who disconnected, 
        /// and it stops its associated APIs, unless another
        /// connected client is using them.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken) {

            _logger.LogInformation($"ApiLauncherWindowsServiceClient is stopping for {_clientId}.");
            _mqttClient.DisconnectAsync(); //disconnect from the Windows service

            return Task.CompletedTask;
        }

        public void Dispose() {
        }
    }
}