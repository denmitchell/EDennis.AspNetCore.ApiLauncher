using EDennis.AspNetCore.Base.Web;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.Samples.Utils {

    /// <summary>
    /// Xunit class fixture used to launch and terminate a web server for integration testing
    /// </summary>
    public class LauncherFixture<TProgram> : IDisposable 
        where TProgram : IProgram, new()
        {

        //the threading mechanism used to remotely terminate launcher apps
        private readonly EventWaitHandle _ewh;

        /// <summary>
        /// An HttpClient that will be used for all tests of the entry-point application
        /// </summary>
        public HttpClient HttpClient { get; }

        public IProgram Program { get; }

        /// <summary>
        /// Constructs a new fixture and sets up the EventWaitHandle for 
        /// remote termination of the entry-point app
        /// </summary>
        public LauncherFixture() {

            //setup the EventWaitHandle
            var arg = $"ewh={Guid.NewGuid().ToString()}";
            _ewh = new EventWaitHandle(false, EventResetMode.ManualReset, arg);

            Program = new TProgram();

            //create the HttpClient
            HttpClient = new HttpClient {
                BaseAddress = new Uri(Program.Api.MainAddress)
            };

            //asynchronously initiate the launch of the server 
            Program.Run(new string[] { arg });

            //optional : use custom PingAsync (see HttpClientExtensions) to wait for server to start.
            var canPing = HttpClient.PingAsync(10).Result;

        }

        /// <summary>
        /// In disposing of this fixture instance, signal the EventWaitHandle so that the
        /// launcher app can terminate and then dispose of the EventWaitHandle.
        /// </summary>
        public void Dispose() {
            _ewh.Set();
            _ewh.Dispose();
        }
    }
}
