using EDennis.Samples.SharedModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace EDennis.Samples.TimeApi {
    public class Program {

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
            config.GetSection("Apis").Bind(apis);
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
}
