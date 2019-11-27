using EDennis.Samples.Utils;
using System.Threading.Tasks;

namespace EDennis.Samples.ApiGateway.LauncherAlt {

    /// <summary>
    /// This launcher runs all APIs from the .Api project's Program.Main method
    /// </summary>
    public class Program : ILauncher {

        public static void Main(string[] args) {
            new Program().Launch(args);
            LauncherUtils.Block();
        }

        public void Launch(string[] args) {
            Task.Run(() => TimeApi.Program.Main(args));
            Task.Run(() => LocationApi.Program.Main(args));
            Task.Run(() => NameApi.Program.Main(args));
            Task.Run(() => QuoteApi.Program.Main(args));
            Task.Run(() => ApiGateway.Program.Main(args));
        }


    }
}
