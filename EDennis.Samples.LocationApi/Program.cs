using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using L = EDennis.Samples.LocationApi.Lib;

namespace EDennis.Samples.LocationApi {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new L.Program().CreateHostBuilder(args);
    }
}
