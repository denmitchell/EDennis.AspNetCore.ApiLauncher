using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using T = EDennis.Samples.TimeApi.Lib;

namespace EDennis.Samples.TimeApi {
    public class Program {

        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new T.Program().CreateHostBuilder(args);

    }
}
