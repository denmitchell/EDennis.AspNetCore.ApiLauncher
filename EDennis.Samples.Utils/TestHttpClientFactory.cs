using EDennis.AspNetCore.Base.Web;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Testing {

    public class TestHttpClientFactory<IStartup> : IHttpClientFactory
        where IStartup : class {

        public Dictionary<string, TestApiFactory<IStartup>> FactoryDictionary { get; set; }


        public HttpClient CreateClient(string name) {
            return FactoryDictionary[name].CreateClient();
        }
    }
}
