using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;

namespace EDennis.Samples.QuoteApi.Tester {
    public class SampleUnitTest : IClassFixture<LauncherFixture> {

        public ITestOutputHelper _output;
        public HttpClient _client;

        public SampleUnitTest(LauncherFixture fixture, ITestOutputHelper output) {
            _client = fixture.HttpClient;
            _output = output;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetQuote(int idx) {
            var result = _client.Get<Quote>($"{_client.BaseAddress}Quote");
            var quote = (Quote)result.Value;
            _output.WriteLine($"{idx}: " + JsonSerializer.Serialize(quote, new JsonSerializerOptions { WriteIndented = true }));
        }


        public static IEnumerable<object[]> Data =>
                Enumerable.Range(1, 10).Select(i => new object[] { i }).ToArray();
    }
}
