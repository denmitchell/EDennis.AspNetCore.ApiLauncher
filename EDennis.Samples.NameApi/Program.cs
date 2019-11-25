using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N = EDennis.Samples.NameApi.Lib;

namespace EDennis.Samples.NameApi {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new N.Program().CreateHostBuilder(args);
    }
}
