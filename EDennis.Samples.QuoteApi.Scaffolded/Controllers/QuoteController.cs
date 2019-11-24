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

namespace EDennis.Samples.QuoteApi.Scaffolded.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class QuoteController : ControllerBase {

        private static readonly Quote[] _Quotes = new Quote[]
        {
            new Quote {Id = 1, Text = "The better part of valor is discretion."},
            new Quote {Id = 2, Text = "All the world's a stage, and all the men and women merely players."},
            new Quote {Id = 3, Text = "To thine own self be true."},
        };

        private readonly ILogger<QuoteController> _logger;
        private readonly HttpClient _httpClient;

        public QuoteController(ILogger<QuoteController> logger, 
            IHttpClientFactory httpClientFactory,
            IOptionsMonitor<Apis> apis) {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("TimeApi");
            var api = apis.CurrentValue.FirstOrDefault(a => a.Value.ProjectName == "EDennis.Samples.TimeApi.Scaffolded").Value;
            _httpClient.BaseAddress = new Uri(api.MainAddress);
        }

        [HttpGet]
        public IEnumerable<Quote> Get() {
            var time = GetTime();
            for (int i = 0; i < _Quotes.Length; i++)
                _Quotes[i].LastAccessed = time;
            return _Quotes;
        }


        [HttpGet("{id}")]
        public Quote Get(int id) {
            var time = GetTime();
            var Quote = _Quotes.FirstOrDefault(n=>n.Id == id);
            Quote.LastAccessed = time;
            return Quote;
        }


        private Time GetTime() {
            _logger.LogInformation("For Quote Api, retrieving Time from TimeApi @ {TimeApiUrl}", _httpClient.BaseAddress.ToString() );
            var response = _httpClient.GetAsync($"{_httpClient.BaseAddress}Time").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var time = JsonSerializer.Deserialize<Time>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return time;
        }

    }
}
