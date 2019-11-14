using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EDennis.Samples.SharedModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.ApiGateway.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class TimeController : ControllerBase {

        private readonly ILogger<TimeController> _logger;
        private readonly HttpClient _httpClient;

        public TimeController(ILogger<TimeController> logger,
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<Apis> apis) {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            var api = apis.CurrentValue.FirstOrDefault(a => a.Value.ProjectName == "EDennis.Samples.TimeApi").Value;
            _httpClient.BaseAddress = new Uri(api.MainAddress);
        }

        [HttpGet]
        public Time Get() {
            _logger.LogInformation("For Api Gateway, retrieving Time from TimeApi @ {TimeApiUrl}", _httpClient.BaseAddress.ToString());
            var response = _httpClient.GetAsync($"{_httpClient.BaseAddress}Time").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var Time = JsonSerializer.Deserialize<Time>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Time;
        }

    }
}
