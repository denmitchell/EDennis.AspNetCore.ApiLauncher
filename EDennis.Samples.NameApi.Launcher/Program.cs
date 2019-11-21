using EDennis.Samples.Utils;
using N = EDennis.Samples.NameApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.NameApi.Launcher {
    public class Program : ILauncher {

        public static void Main(string[] args) {
            new Program().Launch(args);
        }

        public void Launch(string[] args) {
            new T.Program().Run(args);
            new N.Program().Run(args);
        }

    }
}
