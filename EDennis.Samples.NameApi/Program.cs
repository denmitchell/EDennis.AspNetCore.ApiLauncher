using EDennis.Samples.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EDennis.Samples.NameApi {
    public class Program {
        public static async void RunAsync(string[] args) {
            var host = ProgramUtils.CreateHostBuilder<Program, Startup>(args).Build();
            await host.RunAsync();
        }
    }
}
