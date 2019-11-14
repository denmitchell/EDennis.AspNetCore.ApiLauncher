using System;
using System.Threading.Tasks;
using Q = EDennis.Samples.QuoteApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.QuoteApi.Launcher {
    public class Program {

        public static void Main(string[] args) {
            Task.Run(() => { T.Program.RunAsync(args); });
            Task.Run(() => { Q.Program.RunAsync(args); });
            Console.ReadKey();
        }

    }
}
