using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using L = EDennis.Samples.TimeApi.Launcher;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EDennis.Samples.TimeApi.Tester {
    public class LauncherFixture : IDisposable {
        private readonly EventWaitHandle _ewh;

        public HttpClient HttpClient { get; set; } 

        public LauncherFixture() {
            var arg = $"ewh={Guid.NewGuid().ToString()}";
            Task.Run(() => { L.Program.Main(new string[] { arg }); });
            _ewh = new EventWaitHandle(false, EventResetMode.ManualReset, arg);

            HttpClient = new HttpClient {
                BaseAddress = new Uri("https://localhost:6501")
            };

            var canPing = HttpClient.PingAsync(10).Result;

        }
        public void Dispose() {
            _ewh.Set();
        }
    }
}
