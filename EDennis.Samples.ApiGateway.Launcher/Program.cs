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
            Task.Run(() => { new T.Program().RunAsync(args); });
            Task.Run(() => { new L.Program().RunAsync(args); });
            Task.Run(() => { new N.Program().RunAsync(args); });
            Task.Run(() => { new Q.Program().RunAsync(args); });
            Task.Run(() => { new G.Program().RunAsync(args); });
            LauncherUtils.Block(args);
        }

    }
}
