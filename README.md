# EDennis.AspNetCore.ApiLauncher
An ASP.NET Core Web API pattern where Program.cs is divided into two classes in two projects to support launching multiple APIs in a single console and to support APIs as NuGet packages.

## Motivation
The motivation for the ApiLauncher pattern stems from three problems:
1. Often, ASP.NET Core web applications depend upon one or more child APIs.  When integration testing these APIs, it is a little cumbersome to setup Visual Studio to start multiple projects or to manually start the projects, one by one.  Futhermore, one must ensure that the configured ports for the projects are distinct from each other -- which requires inspecting/updating launchSettings.json files in individual projects. An easier way to launch multiple dependent applications was needed.
2. When an application depend upon one or more reusable APIs, a developer needs to be able to debug and test the main project with these reusable APIs.  The simplest approach is to create a solution that includes the main project and project references to the reusable APIs (whose homes are other solutions).  Unfortunately, with this approach, the source code for the reusable APIs can be modified outside of source control.  If the reusable APIs could be published as NuGet packages, then client applications could reference the APIs without the possibility of modifying their source code.

## Microsoft Default Program Class 

In ASP.NET Core web applications, the scaffolded Program class has a well-defined structure consisting of a Main method (the entry point for the application) and a CreateHostBuilder method. The latter method calls CreateDefaultBuilder, which performs some initial setup/configuration, including the identification of the application's Startup class, the identification of the underyling HTTP server (e.g., Kestrel) and the reading of appsettings files.  CreateHostBuilder returns an IHostBuilder, which the Main method Builds and runs (synchronously) as an IHost/WebHost instance.  The typical scaffolded Program.cs (.NET Core 3.0) is displayed below.

```C#
// default scaffolded Program.cs (.NET Core 3.0)
// usings omitted
namespace My.Reusable.Api {    
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
```

## Breaking Up the Program Class 

The Microsoft scaffolded Program.cs is simple, elegant, and quite serviceable; however, by breaking up the class into separate classes that reside in separate projects, one can achieve much great flexibility without too much added complexity.

For pedagogic purposes, let's breakup recompose the Program class in a few steps. 

### Step 1: Moving Main

NuGet packages can contain an entry-point Main method; however, it is a little more difficult to configure/manage the build process when an application has its own Main method and also references one or more reusable APIs with their Main methods.  Let's move the Main method out of the resuable API's Program.cs and place it in a separate Console project that references the reusable API.

```C#
//LAUNCHER CONSOLE APP -- INITIAL VERSION
using Microsoft.Extensions.Hosting; //needed for Run/RunAsync
using A = My.Reusable.Api;
namespace My.Reusable.Api.Launcher {
    // Program.cs in a separate Api Launcher app
    public class Program {
        public static void Main(string[] args) {
            A.Program.CreateHostBuilder(args).Build().Run();
        }
    }
 }
```

Now, the original Program.cs class is fit to be published into a NuGet package.  But this isn't all we can do.  We can use this new Launcher app to launch more than one API.

### Step 2: Running Asynchronously

The default IHost/WebHost Run() method is synchronous, which means that the executing thread will block on the line of code that calls Run().  If you want to launch more than one API, you can wrap the call to IHost/WebHost Run in a Task.Run(()=>{}.  A Console.ReadKey() line will prevent the Console application from immediately exited. 

```C#
//LAUNCHER MODIFICATIONS
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using A = My.Reusable.Api;
using B = My.Other.Reusable.Api;
namespace My.Reusable.Apis.Launcher {
    // Program.cs in a separate Api Launcher app
    public class Program {
        public static void Main(string[] args) {
            Task.Run(() => {
                A.Program.CreateHostBuilder(args).Build().Run();
            });
            Task.Run(() => {
                B.Program.CreateHostBuilder(args).Build().Run();
            });
            Console.ReadKey();
        }
    }
}
```

### Step 3: Simplifying the Launcher 

The above code is fine, but it can be tightened up a bit if we move some of the code back to the reusable API's Program class.  Let's create a RunAsync method in the Reusable API's Program class and call that method from the Launcher Console app.

```C#
//REUSABLE API MODIFICATIONS
namespace My.Reusable.Api {    
    public class Program {
        //asynchronous method that builds and runs the IHost/WebHost
        public static async void RunAsync (string[] args) {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }
        //...
    }
}

//LAUNCHER MODIFICATIONS
//...
        public static void Main(string[] args) {
            Task.Run(() => { A.Program.RunAsync(args); });
            Task.Run(() => { B.Program.RunAsync(args); });
            Console.ReadKey();
        }

//...
```

What is especially nice about this approach is that a single Console app provides the standard and error output for all APIs.  When you have a project with several child APIs, this can be very convenient.

This is simpler, but we aren't quite done.  Although we have centralized the launching of multiple APIs under a single launcher, we would like to centralize the assignment of ports to these APIs, as well.

### Step 4: Centralizing Port Assignments 

To centralize port assignments, you can modify each reusable API's Program.cs to use port assignments from Configuration, which can pull from a centralized source (e.g., a configuration database, a configuration API, or a common file).  The following code assumes that Configuration is structured to support a Dictionary of named configurations for each API, where the API has stored properties that include the Project Name (assembly name) and ports.

Below are the model classes used for binding relevant sections of configuration that hold project/port info.

```C#
    public class Apis : Dictionary<string,Api> { }

    public class Api {
        public string ProjectName { get; set; }
        public string Host { get; set; }
        public string Scheme { get; set; }
        public int? HttpsPort { get; set; }
        public int? HttpPort { get; set; }
        public decimal Version { get; set; }

        public int? MainPort {
            get {
                if (Scheme == null)
                    return null;
                else if (Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                    return HttpsPort;
                else
                    return HttpPort;
            }
        }

        public int? AltPort {
            get {
                if (Scheme == null)
                    return null;
                else if (Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                    return HttpPort;
                else
                    return HttpsPort;
            }
        }

        public string MainAddress { 
            get {
                if (Scheme == null)
                    return null;
                return $"{Scheme}://{Host}:{MainPort}";
            } 
        }

        public string AltAddress {
            get {
                if (Scheme == null)
                    return null;
                return $"{(Scheme.Equals("https",StringComparison.OrdinalIgnoreCase) ? "http": "https")}://{Host}:{AltPort}";
            }
        }


        public string[] Urls {
            get {
                if (Scheme == null)
                    return null;
                else if (Scheme.Equals("http", StringComparison.OrdinalIgnoreCase))
                    return new string[] { MainAddress };
                else
                    return new string[] { MainAddress, AltAddress};
            }
        }
    }
```

Below is a modification to support reading port assignments from Configuration, where the section key is set to "Apis"

```C#

    public class Program {

        public const string APIS_CONFIGURATION_SECTION = "Apis";

        public static async void RunAsync (string[] args) {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) {

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    var configuration = GetConfiguration();
                    var urls = GetUrls(GetApis(configuration));
                    webBuilder
                        .UseConfiguration(configuration)
                        .UseUrls(urls)
                        .UseStartup<Startup>();
                });
            return builder;
        }

        private static IConfiguration GetConfiguration() {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var assembly = typeof(Program).Assembly;
            var provider = new ManifestEmbeddedFileProvider(assembly);
            var config = new ConfigurationBuilder()
                .AddJsonFile(provider, $"appsettings.json", true, true)
                .AddJsonFile(provider, $"appsettings.{env}.json", true, true)
                .AddJsonFile($"appsettings.Shared.json", true)
                .Build();
            return config;
        }

        private static Apis GetApis(IConfiguration config) {
            var apis = new Apis();
            config.GetSection(APIS_CONFIGURATION_SECTION).Bind(apis);
            return apis;
        }

        private static string[] GetUrls(Apis apis) {
            var api = apis.FirstOrDefault(a => a.Value.ProjectName == typeof(Program).Assembly.GetName().Name).Value;
            return api.Urls;
        }

        private static string[] GetApiKeys(string[] args) {
            Regex pattern = new Regex("/[A-Za-z0-9_]+$");
            return args.Where(a => pattern.IsMatch(a)).Select(a => a.Substring(1)).ToArray();
        }


    }

```

### Step 5: Embedding Configurations in NuGet Packages 

In the above, code I made use of a ManifestEmbeddedFileProvider for the two regular appsettings files.  Also, I included a separate appsettings.Shared.json file.  The reason for this approach is to allow NuGet to hold some partial configurations for the reusable APIs but to allow the reusable APIs to complete the configurations from a common source file.  

The partial configurations are contained in appsettings.json and appsettings.{env}.json, which are embedded in the NuGet packages.  For configurations that don't change within a given environment (e.g., database connection strings and security settings for a development environment), it makes sense to embed these settings in the NuGet package.  Microsoft provides the [ManifestEmbeddedFileProvider](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/file-providers?view=aspnetcore-3.0#manifestembeddedfileprovider) that allows developers to read in configurations from embedded files.  Note that there is a little setup work in .csproj to support the embedding. 

Here is a .csproj file from one of the sample reusable API projects in this repository:

```XML
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <GenerateEmbeddedFilesManifest>true</GenerateEmbeddedFilesManifest>
    <OutputType>library</OutputType>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.FileProviders.Embedded" Version="3.0.0" />
  </ItemGroup>

  <ItemGroup>
    <EmbeddedResource Include="appsettings.Development.json" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\EDennis.Samples.SharedModel\EDennis.Samples.SharedModel.csproj" />
  </ItemGroup>

</Project>
```

For configurations that should be controlled outside of NuGet (e.g., port assignments on a local machine), one can have NuGet packages reference a *to-be-provided* appsettings.Shared.json file which can reside in the Launcher project.  Note that the call to AddJsonFile can specify that the resource is optional, just in case no centralized configurations are required.

In the sample projects, I embedded certain configurations for the APIs (ProjectName, Scheme, and Host) and centralize other configurations (HttpsPort and HttpPort).  Assuming the API configuration keys have the same name (e.g, TimeApi, LocationApi), the two configurations -- embedded and centralized -- are merged into single API objects during binding. 
