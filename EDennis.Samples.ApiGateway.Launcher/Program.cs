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
            new T.Program().Run(args);
            new L.Program().Run(args);
            new N.Program().Run(args);
            new Q.Program().Run(args);
            new G.Program().Run(args);
            LauncherUtils.Block(args);
        }

    }
}
