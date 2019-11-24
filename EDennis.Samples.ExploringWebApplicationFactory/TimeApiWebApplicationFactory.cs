using EDennis.Samples.SharedModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.Samples.ExploringWebApplicationFactory {
    public class TimeApiWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class {
        
        public Api Api { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder) {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();
            var apis = new Apis();
            configuration.GetSection("Apis").Bind(apis);
            Api = apis["TimeApi"];

            builder.UseUrls(Api.Urls);

        }


    }
}
