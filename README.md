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
This is simpler, but we aren't quite done.  ... 

