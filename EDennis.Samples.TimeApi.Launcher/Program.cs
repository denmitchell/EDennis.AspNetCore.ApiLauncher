using System;
using System.Threading.Tasks;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.TimeApi.Launcher {

    class Program {
        public static void Main(string[] args) {
            Task.Run(() => { T.Program.RunAsync(args); });
            Console.ReadKey();
        }
    }
}
