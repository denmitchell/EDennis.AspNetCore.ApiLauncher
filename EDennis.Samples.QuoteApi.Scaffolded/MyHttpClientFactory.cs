using System.Net.Http;

namespace EDennis.Samples.QuoteApi.Scaffolded {
    public class MyHttpClientFactory : IHttpClientFactory {
        public HttpClient CreateClient(string name) {
            return new HttpClient();
        }
    }
}