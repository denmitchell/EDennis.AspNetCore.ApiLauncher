using EDennis.Samples.Utils;
using L = EDennis.Samples.LocationApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.LocationApi.Launcher {
    public class Program : ILauncher {

        public static void Main(string[] args) {
            new Program().Launch(args);
        }

        public void Launch(string[] args) {
            new T.Program().Run(args);
            new L.Program().Run(args);
            LauncherUtils.Block(args);
        }


    }
}
