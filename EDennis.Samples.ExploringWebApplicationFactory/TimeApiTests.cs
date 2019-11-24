using EDennis.Samples.SharedModel;
using EDennis.Samples.TimeApi.Scaffolded;
using EDennis.Samples.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.ExploringWebApplicationFactory {
    public class TimeApiTests : IClassFixture<TimeApiWebApplicationFactory<Startup>> {


        private readonly TimeApiWebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;
        public TimeApiTests(TimeApiWebApplicationFactory<Startup> factory, ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void Test1() {
            var client = _factory.CreateClient();
            var result = client.Get<Time>($"Time");
            var time = (Time)result.Value;
            if (time == null)
                throw new ApplicationException($"Cannot retrieve time from endpoint: {client.BaseAddress}Time");
            _output.WriteLine(JsonSerializer.Serialize(time, new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
