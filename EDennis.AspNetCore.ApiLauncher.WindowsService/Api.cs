using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EDennis.AspNetCore.ApiLauncher{

    /// <summary>
    /// This class holds all properties related to a an API,
    /// including project name and network address/port, 
    /// whether the Api is ready, and optional command-line
    /// arguments
    /// </summary>
    public class Api {

        //the path to ... source/repos
        public string RepoDirectory { get; set; }
            = $"C:\\Users\\{Environment.UserName}\\source\\repos\\";

        //the name of the project (typically also the name of the project directory)
        public string ProjectName { get; set; }
        
        //the name of the solution (typically also the name of the solution folder)
        public string SolutionName { get; set; }

        //optional command-line arguments as 
        //semicolon-delimited key-value pairs joined by equals
        //(e.g., MockClient=Client1)
        public string CommandLineArgs { get; set; }

        //network address (including port) for API
        public string BaseAddress { get; set; }

        //whether the API is ready to use
        public bool? Ready { get; set; }

        //the port number for API
        public int Port { get; set; }

        //the client applications that need the API
        //disallow deserialization
        [JsonIgnore]
        public List<string> Clients { get; set; } = new List<string>();

        //the client applications that need the API
        //allow serialization
        [JsonProperty("Clients")]
        List<string> GetClients { get { return Clients; } }

        //The OS process used to launch the API
        [JsonIgnore]
        public Process Process { get; set; }

        //this property allows overriding the default path to a project
        public string FullProjectPath { get; set; }

        //the path to the project on the development computer
        public string LocalProjectDirectory {
            get {
                if (FullProjectPath == null)
                    return $"{RepoDirectory}{SolutionName}\\{ProjectName}";
                else
                    return FullProjectPath;
            }
            set {

            }
        }

    }

}
