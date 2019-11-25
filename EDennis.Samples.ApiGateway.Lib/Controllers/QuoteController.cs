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
    public class QuoteController : ControllerBase {

        private readonly ILogger<QuoteController> _logger;
        private readonly HttpClient _httpClient;

        public QuoteController(ILogger<QuoteController> logger,
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<Apis> apis) {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("QuoteApi");
            var api = apis.CurrentValue.FirstOrDefault(a => a.Value.ProjectName == "EDennis.Samples.QuoteApi").Value;
            _httpClient.BaseAddress = new Uri(api.MainAddress);
        }

        [HttpGet]
        public IEnumerable<Quote> Get() {
            _logger.LogInformation("For Api Gateway, retrieving Quotes from QuoteApi @ {QuoteApiUrl}", $"{_httpClient.BaseAddress.ToString()}/Quote");
            var response = _httpClient.GetAsync($"Quote").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var Quote = JsonSerializer.Deserialize<List<Quote>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Quote;
        }


        [HttpGet("{id}")]
        public Quote Get(int id) {
            _logger.LogInformation("For Api Gateway, retrieving Quote from QuoteApi @ {QuoteApiUrl}", $"{_httpClient.BaseAddress.ToString()}Quote/{id}");
            var response = _httpClient.GetAsync($"Quote/{id}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var Quote = JsonSerializer.Deserialize<Quote>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Quote;
        }

    }
}
