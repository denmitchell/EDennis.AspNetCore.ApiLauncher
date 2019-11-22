using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using L = EDennis.Samples.LocationApi;

namespace EDennis.Samples.LocationApi.Host {
    public class Program {
        public static void Main(string[] args) {
            new L.Program().CreateHostBuilder(args).Build().Run();
        }
    }
}
