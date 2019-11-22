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

namespace EDennis.Samples.LocationApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase {

        private static readonly Location[] _locations = new Location[]
        {
            new Location {Id = 1, City = "Hartford", State = "Connecticut"},
            new Location {Id = 2, City = "Boston", State = "Massachusetts"},
            new Location {Id = 3, City = "Providence", State = "Rhode Island"},
        };

        private readonly ILogger<LocationController> _logger;
        private readonly HttpClient _httpClient;

        public LocationController(ILogger<LocationController> logger, 
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<Apis> apis) {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            var api = apis.CurrentValue.FirstOrDefault(a => a.Value.ProjectName == "EDennis.Samples.TimeApi").Value;
            _httpClient.BaseAddress = new Uri(api.MainAddress);
        }

        [HttpGet]
        public IEnumerable<Location> Get() {
            var time = GetTime();
            for (int i = 0; i < _locations.Length; i++)
                _locations[i].LastAccessed = time;
            return _locations;
        }


        [HttpGet("{id}")]
        public Location Get(int id) {
            var Location = _locations.FirstOrDefault(n=>n.Id == id);
            Location.LastAccessed = GetTime();
            return Location;
        }


        private Time GetTime() {
            _logger.LogInformation("For Name Api, retrieving Time from TimeApi @ {TimeApiUrl}", _httpClient.BaseAddress.ToString());
            var response = _httpClient.GetAsync("Time").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var time = JsonSerializer.Deserialize<Time>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return time;
        }

    }
}
