using EDennis.Samples.SharedModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Collections.Generic;
using System;
using System.Linq;

namespace EDennis.AspNetCore.Base.Testing {

    public class TestApi<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class {

        private readonly Dictionary<string, Func<HttpClient>> _create;
        private readonly IConfiguration _configuration;
        private readonly string _apisConfigurationKey;

        public TestApi(Dictionary<string, Func<HttpClient>> create,
            Dictionary<string,Action> dispose,
            IConfiguration configuration, string apisConfigurationKey,
            Apis apis) {
            _create = create;
            _configuration = configuration;
            _apisConfigurationKey = apisConfigurationKey;

            var projName = typeof(TEntryPoint).Assembly.GetName().Name;
            var apiKey = apis.FirstOrDefault(a => a.Value.ProjectName == projName).Key;
            create.Add(apiKey, CreateClient);
            dispose.Add(apiKey, Dispose);

            var _ = Server; //ensure creation;

        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) {

            builder.ConfigureServices(services => {
                services.Configure<Apis>(_configuration.GetSection(_apisConfigurationKey));

                services.AddScoped<IHttpClientFactory>(provider => {
                    return new TestHttpClientFactory(_create);
                });
                foreach(var apiKey in _create.Keys)
                    services.AddHttpClient(apiKey);

                services.AddControllers();
            });

        }


    }


}
