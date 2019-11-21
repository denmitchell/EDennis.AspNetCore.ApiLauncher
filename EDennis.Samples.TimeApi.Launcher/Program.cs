using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.Utils;
using System.Threading.Tasks;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.TimeApi.Launcher {

    public class Program : ILauncher {
        public static void Main(string[] args) {
            new Program().Launch(args);
            LauncherUtils.Block();
        }

        public void Launch(string[] args) {
            var t = new T.Program().Run(args);
            ProgramBase.CanPingAsync(t);
        }
    }

}
