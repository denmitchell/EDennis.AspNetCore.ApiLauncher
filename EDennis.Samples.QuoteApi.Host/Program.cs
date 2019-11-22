using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Q = EDennis.Samples.QuoteApi;

namespace EDennis.Samples.QuoteApi.Host {
    public class Program {
        public static void Main(string[] args) {
            new Q.Program().CreateHostBuilder(args).Build().Run();
        }
    }
}
