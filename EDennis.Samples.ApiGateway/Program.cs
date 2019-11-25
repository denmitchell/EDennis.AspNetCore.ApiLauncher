using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using G = EDennis.Samples.ApiGateway.Lib;

namespace EDennis.Samples.ApiGateway {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new G.Program().CreateHostBuilder(args);
    }
}
