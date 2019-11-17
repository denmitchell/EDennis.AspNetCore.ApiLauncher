using EDennis.Samples.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EDennis.Samples.TimeApi {
    public class Program {

        public static async void RunAsync (string[] args) {
            var host = ProgramUtils.CreateHostBuilder<Program, Startup>(args).Build();
            await host.RunAsync();
        }


        #region if using your own configuration providers ...
        ////use the call below if you need to use your own configuration providers
        //public static async void RunAsync(string[] args) {
        //    var host = ProgramUtils.CreateHostBuilder<Program,Startup>(args,GetConfiguration<Program>()).Build();
        //    await host.RunAsync();
        //}

        ///// <summary>
        ///// Use this to add your own configuration
        ///// </summary>
        ///// <typeparam name="TProgram"></typeparam>
        ///// <returns></returns>
        //public static IConfiguration GetConfiguration<TProgram>()
        //    where TProgram : class {
        //    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        //    var assembly = typeof(TProgram).Assembly;
        //    var provider = new ManifestEmbeddedFileProvider(assembly);
        //    var config = new ConfigurationBuilder()
        //        .AddJsonFile(provider, $"appsettings.json", true, true)
        //        .AddJsonFile(provider, $"appsettings.{env}.json", true, true)
        //        .AddJsonFile($"appsettings.Shared.json", true)
        //        .Build();
        //    return config;
        //}

        #endregion

    }
}
