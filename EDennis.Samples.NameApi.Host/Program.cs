using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N = EDennis.Samples.NameApi;

namespace EDennis.Samples.NameApi.Host {
    public class Program {
        public static void Main(string[] args) {
            new N.Program().CreateHostBuilder(args).Build().Run();
        }
    }
}
