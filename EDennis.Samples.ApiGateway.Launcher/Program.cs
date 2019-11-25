using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Utils;
using G = EDennis.Samples.ApiGateway.Lib;
using L = EDennis.Samples.LocationApi.Lib;
using N = EDennis.Samples.NameApi.Lib;
using Q = EDennis.Samples.QuoteApi.Lib;
using T = EDennis.Samples.TimeApi.Lib;

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
