using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Utils;
using Q = EDennis.Samples.QuoteApi.Lib;
using T = EDennis.Samples.TimeApi.Lib;

namespace EDennis.Samples.QuoteApi.Launcher {
    public class Program : ILauncher {
        public static void Main(string[] args) {
            new Program().Launch(args);
            LauncherUtils.Block();
        }
        public void Launch(string[] args) {
            var t = new T.Program().Run(args);
            var q = new Q.Program().Run(args);
            ProgramBase.CanPingAsync(t, q);
        }

    }
}
