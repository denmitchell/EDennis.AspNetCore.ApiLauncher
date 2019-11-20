using EDennis.Samples.Utils;
using System.Threading.Tasks;
using N = EDennis.Samples.NameApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.NameApi.Launcher {
    public class Program {

        public static void Main(string[] args) {
            Task.Run(() => { new T.Program().RunAsync(args); });
            Task.Run(() => { new N.Program().RunAsync(args); });
            LauncherUtils.Block(args);
        }

    }
}
