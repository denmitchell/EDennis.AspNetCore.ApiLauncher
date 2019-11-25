using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Utils;
using N = EDennis.Samples.NameApi.Lib;
using T = EDennis.Samples.TimeApi.Lib;

namespace EDennis.Samples.NameApi.Launcher {
    public class Program : ILauncher {

        public static void Main(string[] args) {
            new Program().Launch(args);
            LauncherUtils.Block();
        }

        public void Launch(string[] args) {
            var t = new T.Program().Run(args);
            var n = new N.Program().Run(args);
            ProgramBase.CanPingAsync(t, n);
        }

    }
}
