using Flurl;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EDennis.Samples.Utils {
    public static class HttpClientExtensions {


        public static ObjectResult Get<TResponseObject>(this HttpClient client, string relativeUrlFromBase) {
            return client.GetAsync<TResponseObject>(relativeUrlFromBase).Result;
        }

        public static async Task<ObjectResult> GetAsync<TResponseObject>(
                this HttpClient client, string relativeUrlFromBase) {


            var url = Url.Combine(client.BaseAddress.ToString(), relativeUrlFromBase);
            var response = await client.GetAsync(url);
            var objResult = await GenerateObjectResult<TResponseObject>(response);

            return objResult;

        }

        private async static Task<ObjectResult> GenerateObjectResult<T>(HttpResponseMessage response) {

            object value = null;

            int statusCode = (int)response.StatusCode;

            if (response.Content.Headers.ContentLength > 0) {
                var json = await response.Content.ReadAsStringAsync();

                if (statusCode < 299 && typeof(T) != typeof(string)) {
                    value = JsonSerializer.Deserialize<T>(json);
                } else {
                    value = json;
                }
            }

            return new ObjectResult(value) {
                StatusCode = statusCode
            };

        }


        public static bool Ping(this HttpClient client, int timeoutSeconds = 5) {
            return client.PingAsync(timeoutSeconds).Result;
        }


        public static async Task<bool> PingAsync(this HttpClient client, int timeoutSeconds = 5) {

            var pingable = false;

            await Task.Run(() => {

                var port = client.BaseAddress.Port;
                var host = client.BaseAddress.Host;
                var sw = new Stopwatch();

                sw.Start();
                while (sw.ElapsedMilliseconds < (timeoutSeconds * 1000)) {
                    try {
                        using var tcp = new TcpClient(host, port);
                        var connected = tcp.Connected;
                        pingable = true;
                        break;
                    } catch (Exception ex) {
                        if (!ex.Message.Contains("No connection could be made because the target machine actively refused it"))
                            throw ex;
                        else
                            Thread.Sleep(1000);
                    }

                }

            });
            return pingable;
        }


    }
}
