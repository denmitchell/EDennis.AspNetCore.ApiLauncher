using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using G = EDennis.Samples.ApiGateway;

namespace EDennis.Samples.ApiGateway.Host {
    public class Program {
        public static void Main(string[] args) {
            new G.Program().CreateHostBuilder(args).Build().Run();
        }
    }
}
