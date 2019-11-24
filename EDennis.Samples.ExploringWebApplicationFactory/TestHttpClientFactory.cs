using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.TimeApi.Scaffolded;

namespace EDennis.Samples.ExploringWebApplicationFactory {

    public class TestHttpClientFactory: IHttpClientFactory {

        private readonly TimeApiWebApplicationFactory<Startup> _timeApiFactory;
        private readonly Apis _apis;

        public TestHttpClientFactory(Apis apis, TimeApiWebApplicationFactory<Startup> timeApiFactory) {
            _apis = apis;
            _timeApiFactory = timeApiFactory;
        }

        public HttpClient CreateClient(string name) {
            HttpClient client = null;
            if (name == "TimeApi") {
                var uri = new Uri(_apis[name].MainAddress);
                client = _timeApiFactory.CreateClient(options: new WebApplicationFactoryClientOptions { BaseAddress = uri });
            }
            return client;
        }
    }
}
