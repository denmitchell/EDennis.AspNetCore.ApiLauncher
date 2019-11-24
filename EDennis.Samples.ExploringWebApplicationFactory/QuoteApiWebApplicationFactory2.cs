using EDennis.Samples.SharedModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using EDennis.Samples.TimeApi.Scaffolded;

namespace EDennis.Samples.ExploringWebApplicationFactory {
    public class QuoteApiWebApplicationFactory2<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class {

        public Api Api { get; private set; }
        public Apis Apis { get; private set; }

        public TimeApiWebApplicationFactory<Startup> TimeApiWebApplicationFactory {get; set;}

        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();
            Apis = new Apis();
            configuration.GetSection("Apis").Bind(Apis);
            Api = Apis["QuoteApi"];

            builder.UseUrls(Api.Urls);

            builder.ConfigureServices(services => {
                services.Configure<Apis>(configuration.GetSection("Apis"));
                services.AddScoped<IHttpClientFactory>(provider => {
                    return new TestHttpClientFactory(Apis,TimeApiWebApplicationFactory);
                });
                services.AddHttpClient("TimeApi");
                services.AddControllers();
            });

        }


    }
}
