using System;
using System.Threading.Tasks;
using L = EDennis.Samples.LocationApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.LocationApi.Launcher {
    public class Program {

        public static void Main(string[] args) {
            Task.Run(() => { T.Program.RunAsync(args); });
            Task.Run(() => { L.Program.RunAsync(args); });
            Console.ReadKey();
        }

    }
}
