using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.Samples.Utils {
    public abstract class AbstractLauncherFixture : IDisposable {
        private readonly EventWaitHandle _ewh;

        public abstract int Port { get; }
        public abstract Action<string[]> Main { get; }

        public HttpClient HttpClient { get; set; } 

        public AbstractLauncherFixture() {
            var arg = $"ewh={Guid.NewGuid().ToString()}";
            Task.Run(() => { Main(new string[] { arg }); });
            _ewh = new EventWaitHandle(false, EventResetMode.ManualReset, arg);

            HttpClient = new HttpClient {
                BaseAddress = new Uri($"https://localhost:{Port}")
            };

            var canPing = HttpClient.PingAsync(10).Result;

        }
        public void Dispose() {
            _ewh.Set();
            _ewh.Dispose();
        }
    }
}
