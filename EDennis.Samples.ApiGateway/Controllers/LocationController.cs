using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using EDennis.Samples.SharedModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.Samples.ApiGateway.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase {

        private readonly ILogger<LocationController> _logger;
        private readonly HttpClient _httpClient;

        public LocationController(ILogger<LocationController> logger, 
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<Apis> apis) {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            var api = apis.CurrentValue.FirstOrDefault(a => a.Value.ProjectName == "EDennis.Samples.LocationApi").Value;
            _httpClient.BaseAddress = new Uri(api.MainAddress);
        }

        [HttpGet]
        public IEnumerable<Location> Get() {
            _logger.LogInformation("For Api Gateway, retrieving Locations from LocationApi @ {LocationApiUrl}", _httpClient.BaseAddress.ToString());
            var response = _httpClient.GetAsync($"{_httpClient.BaseAddress}Location").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var location = JsonSerializer.Deserialize<List<Location>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return location;
        }


        [HttpGet("{id}")]
        public Location Get(int id) {
            _logger.LogInformation("For Api Gateway, retrieving Location from LocationApi @ {LocationApiUrl}", $"{_httpClient.BaseAddress.ToString()}/{id}");
            var response = _httpClient.GetAsync($"{_httpClient.BaseAddress}Location/{id}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var location = JsonSerializer.Deserialize<Location>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return location;
        }

    }
}
