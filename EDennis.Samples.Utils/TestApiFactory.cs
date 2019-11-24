using EDennis.Samples.SharedModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Collections.Generic;
using EDennis.AspNetCore.Base.Testing;

namespace EDennis.AspNetCore.Base.Web {
    public class TestApiFactory<IStartup> : WebApplicationFactory<IStartup>
        where IStartup: class {

        /// <summary>
        /// need to populate keys with null objects before anything else
        /// </summary>
        public Dictionary<string, TestApiFactory<IStartup>> FactoryDictionary { get; set; }

        public TestApiFactory(Dictionary<string, TestApiFactory<IStartup>> factoryDictionary) {
            FactoryDictionary = factoryDictionary;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();

            builder.ConfigureServices(services => {
                services.Configure<Apis>(configuration.GetSection("Apis"));

                services.AddScoped<IHttpClientFactory>(provider => {
                    return new TestHttpClientFactory<IStartup>() { FactoryDictionary = FactoryDictionary };
                });
                foreach(var apiKey in FactoryDictionary.Keys)
                    services.AddHttpClient(apiKey);

                services.AddControllers();
            });

        }


    }


}
