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

namespace EDennis.Samples.NameApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class NameController : ControllerBase {

        private static readonly Name[] _names = new Name[]
                {
            new Name {Id = 1, First = "Jack", Last = "Hill"},
            new Name {Id = 2, First = "Jill", Last = "Hill"}
                };

        private readonly ILogger<NameController> _logger;
        private readonly HttpClient _httpClient;

        public NameController(ILogger<NameController> logger,
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<Apis> apis) {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("TimeApi");
            var api = apis.CurrentValue.FirstOrDefault(a => a.Value.ProjectName == "EDennis.Samples.TimeApi").Value;
            _httpClient.BaseAddress = new Uri(api.MainAddress);
        }

        [HttpGet]
        public IEnumerable<Name> Get() {
            var time = GetTime();
            for (int i = 0; i < _names.Length; i++)
                _names[i].LastAccessed = time;
            return _names;
        }


        [HttpGet("{id}")]
        public Name Get(int id) {
            var name = _names.FirstOrDefault(n => n.Id == id);
            name.LastAccessed = GetTime();
            return name;
        }



        private Time GetTime() {
            _logger.LogInformation("For Name Api, retrieving Time from TimeApi @ {TimeApiUrl}", _httpClient.BaseAddress.ToString());
            var response = _httpClient.GetAsync($"{_httpClient.BaseAddress}Time").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var time = JsonSerializer.Deserialize<Time>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return time;
        }

    }
}

