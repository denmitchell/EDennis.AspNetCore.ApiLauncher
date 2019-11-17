using System;
using System.Threading.Tasks;
using T = EDennis.Samples.TimeApi;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace EDennis.Samples.TimeApi.Launcher {

    public class Program {
        public static void Main(string[] args) {
            Task.Run(() => { T.Program.RunAsync(args); });
            Block(args);
        }

        private static void Block(string[] args) {

            Regex pattern = new Regex("(?<=ewh[= ])[A-Za-z0-9\\-]+");
            foreach (var match in args.Where(a => pattern.IsMatch(a)).Select(a => pattern.Match(a))) {
                var guid = match.Value;
                using EventWaitHandle ewh = new EventWaitHandle(
                                true, EventResetMode.ManualReset, guid);
                Console.WriteLine($"{new string('-',80)}\nRunning until EventWaitHandle {guid} is set\n{new string('-', 80)}");
                ewh.WaitOne();
                return;
            }
            Console.WriteLine($"{ new string('-', 60)}\nRunning until any key is pressed\n{new string('-', 60)}");
            Console.ReadKey();
            return;
        }

    }
}
