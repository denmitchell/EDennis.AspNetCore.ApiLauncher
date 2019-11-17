using EDennis.Samples.Utils;
using System;
using System.Threading.Tasks;
using G = EDennis.Samples.ApiGateway;
using L = EDennis.Samples.LocationApi;
using N = EDennis.Samples.NameApi;
using Q = EDennis.Samples.QuoteApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.ApiGateway.Launcher {
    public class Program {

        public static void Main(string[] args) {
            Task.Run(() => { T.Program.RunAsync(args); });
            Task.Run(() => { L.Program.RunAsync(args); });
            Task.Run(() => { N.Program.RunAsync(args); });
            Task.Run(() => { Q.Program.RunAsync(args); });
            Task.Run(() => { G.Program.RunAsync(args); });
            LauncherUtils.Block(args);
        }

    }
}
