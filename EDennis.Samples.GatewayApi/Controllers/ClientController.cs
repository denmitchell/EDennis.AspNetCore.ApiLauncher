using EDennis.AspNetCore.ApiLauncher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.GatewayApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ClientController(IConfiguration config) {
            _config = config;
        }

        [HttpGet]
        public ActionResult<List<Client>> Get() {
            var client = new Client { ClientId = _config["MockClient"] };
            var apis = new Dictionary<string, Api>();
            _config.GetSection("Apis").Bind(apis);

            var clients = new List<Client>();
            foreach (var api in apis) {

                HttpClient httpClient = HttpClientFactory.Create();
                httpClient.BaseAddress = new Uri($"http://localhost:{api.Value.Port}/api/client");
                var response = httpClient.GetAsync(httpClient.BaseAddress.ToString()).Result.Content.ReadAsAsync<Client>().Result;
                clients.Add(response);
            }
            return clients;
        }

    }
}