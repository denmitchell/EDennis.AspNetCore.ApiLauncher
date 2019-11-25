using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Q = EDennis.Samples.QuoteApi.Lib;

namespace EDennis.Samples.QuoteApi {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new Q.Program().CreateHostBuilder(args);
    }
}
