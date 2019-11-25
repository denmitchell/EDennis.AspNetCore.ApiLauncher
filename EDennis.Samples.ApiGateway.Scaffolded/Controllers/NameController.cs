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
    public class NameController : ControllerBase {

        private readonly ILogger<NameController> _logger;
        private readonly HttpClient _httpClient;

        public NameController(ILogger<NameController> logger,
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<Apis> apis) {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("NameApi");
            var api = apis.CurrentValue.FirstOrDefault(a => a.Value.ProjectName == "EDennis.Samples.NameApi.Scaffolded").Value;
            _httpClient.BaseAddress = new Uri(api.MainAddress);
        }

        [HttpGet]
        public IEnumerable<Name> Get() {
            _logger.LogInformation("For Api Gateway, retrieving Names from NameApi @ {NameApiUrl}", $"{_httpClient.BaseAddress.ToString()}Name");
            var response = _httpClient.GetAsync("Name").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var Name = JsonSerializer.Deserialize<List<Name>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Name;
        }


        [HttpGet("{id}")]
        public Name Get(int id) {
            _logger.LogInformation("For Api Gateway, retrieving Name from NameApi @ {NameApiUrl}", $"{_httpClient.BaseAddress.ToString()}Name/{id}");
            var response = _httpClient.GetAsync($"Name/{id}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var Name = JsonSerializer.Deserialize<Name>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Name;
        }

    }
}
