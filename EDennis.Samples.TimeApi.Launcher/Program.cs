using EDennis.Samples.Utils;
using System.Threading.Tasks;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.TimeApi.Launcher {

    public class Program {
        public static void Main(string[] args) {
            Task.Run(() => { new T.Program().RunAsync(args); });
            LauncherUtils.Block(args);
        }
    }

}
