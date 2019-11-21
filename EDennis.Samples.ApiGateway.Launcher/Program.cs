using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Utils;
using System;
using System.Threading.Tasks;
using G = EDennis.Samples.ApiGateway;
using L = EDennis.Samples.LocationApi;
using N = EDennis.Samples.NameApi;
using Q = EDennis.Samples.QuoteApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.ApiGateway.Launcher {
    public class Program : ILauncher {

        public static void Main(string[] args) {
            new Program().Launch(args);
            LauncherUtils.Block();
        }

        public void Launch(string[] args) {
            var t = new T.Program().Run(args);
            var l = new L.Program().Run(args);
            var n = new N.Program().Run(args);
            var q = new Q.Program().Run(args);
            var g = new G.Program().Run(args);
            ProgramBase.CanPingAsync(t, l, n, q, g);
        }


    }
}
