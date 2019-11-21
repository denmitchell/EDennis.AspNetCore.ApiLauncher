using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Utils;
using L = EDennis.Samples.LocationApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.LocationApi.Launcher {
    public class Program : ILauncher {

        public static void Main(string[] args) {
            new Program().Launch(args);
            LauncherUtils.Block();
        }

        public void Launch(string[] args) {
            var t = new T.Program().Run(args);
            var l = new L.Program().Run(args);
            ProgramBase.CanPingAsync(t, l);
        }


    }
}
