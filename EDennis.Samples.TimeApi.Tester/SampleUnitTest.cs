using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using EDennis.Samples.SharedModel;

namespace EDennis.Samples.TimeApi.Tester {
    public class SampleUnitTest : IClassFixture<LauncherFixture> {

        public ITestOutputHelper _output;
        public HttpClient _client;

        public SampleUnitTest(LauncherFixture fixture, ITestOutputHelper output) {
            _client = fixture.HttpClient;
            _output = output;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void MockTest(int idx) {
            var response = _client.GetAsync($"{_client.BaseAddress}Time").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var time = JsonSerializer.Deserialize<Time>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            _output.WriteLine($"{idx}: " + JsonSerializer.Serialize(time, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static IEnumerable<object[]> Data =>
                Enumerable.Range(1, 10).Select(i => new object[] { (object) i }).ToArray();
    }
}
