using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.ApiLauncher {

    /// <summary>
    /// This class is used to wait for a set of APIs to be up and running.
    /// Note: this should be used for testing purposes only.
    /// </summary>
    public static class ApiAwaiter {

        public const int MAX_TIMEOUT_IN_SECONDS = 30;

        /// <summary>
        /// This method waits for a set of APIs to be available, as
        /// evidenced by results from "pinging" localhost at the
        /// appropriate TCP ports.
        /// </summary>
        /// <param name="ports">The ports to check</param>
        /// <param name="logger">A logger that logs port availability</param>
        internal static void AwaitApis(List<int> ports, ILogger logger) {
            //wait for all asynchronous pings to finish.
            Task.WhenAll(ports.Select(p => AwaitApi(p, logger))).Wait();
        }

        /// <summary>
        /// Attempts to connect to a specific TCP port and times out if
        /// it cannot connect before the defined timeout threshold.
        /// </summary>
        /// <param name="port">The port to ping</param>
        /// <param name="logger">A logger to use for logging</param>
        /// <returns></returns>
        private static async Task AwaitApi(int port, ILogger logger) {

            //run asynchrously
            await Task.Run(() => {

                logger.LogInformation("Trying to connect to localhost @ {port}", port);

                //keep track of elapsed time
                var sw = new Stopwatch();
                sw.Start();

                //loop while the elapsed time is under the defined threshold
                while (sw.ElapsedMilliseconds < (MAX_TIMEOUT_IN_SECONDS*1000)) {

                    //when a TCP connection cannot be established, the client
                    //throws an exception.  Handle this exception to keep trying to connect
                    try {
                        //try to connect
                        using (var client = new TcpClient("localhost", port)) {
                            var connected = client.Connected;
                            //if connected, log it, stop the stopwatch, and return
                            logger.LogInformation("Connected to localhost @ {port}", port);
                            sw.Stop();
                            return;
                        }
                    } catch (Exception ex) {
                        //only ignore an exception concerning a failed connection
                        if (!ex.Message.Contains("No connection could be made because the target machine actively refused it"))
                            throw ex;
                        //otherwise, sleep for one second and continue
                        else {
                            logger.LogInformation("Waiting for localhost @ port {port}", port);
                            Thread.Sleep(1000);
                        }
                    }
                }
                sw.Stop();
                //if the API cannot be reached, throw an exception.
                throw new ApplicationException($"Cannot connect to to localhost @ {port}");
            });
        }
    }
}
