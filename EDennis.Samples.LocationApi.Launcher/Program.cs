using EDennis.Samples.Utils;
using System.Threading.Tasks;
using L = EDennis.Samples.LocationApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.LocationApi.Launcher {
    public class Program {

        public static void Main(string[] args) {
            Task.Run(() => { new T.Program().RunAsync(args); });
            Task.Run(() => { new L.Program().RunAsync(args); });
            LauncherUtils.Block(args);
        }

    }
}
