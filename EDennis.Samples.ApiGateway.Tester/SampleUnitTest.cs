using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using EDennis.AspNetCore.Base.Testing;

namespace EDennis.Samples.ApiGateway.Tester {
    public class SampleUnitTest : UnitTestBase<Program> {

        public SampleUnitTest(LauncherFixture<Program> fixture,
            ITestOutputHelper output) : base(fixture, output) {
        }


        [Theory]
        [MemberData(nameof(Data))]
        public void GetTime(int idx) {
            var result = HttpClient.Get<Time>($"{HttpClient.BaseAddress}Time");
            var time = (Time)result.Value;
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(time, new JsonSerializerOptions { WriteIndented = true }));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetLocation(int idx) {
            var result = HttpClient.Get<Location>($"{HttpClient.BaseAddress}Location");
            var location = (Location)result.Value;
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(location, new JsonSerializerOptions { WriteIndented = true }));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetQuote(int idx) {
            var result = HttpClient.Get<Quote>($"{HttpClient.BaseAddress}Quote");
            var quote = (Quote)result.Value;
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(quote, new JsonSerializerOptions { WriteIndented = true }));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetName(int idx) {
            var result = HttpClient.Get<Name>($"{HttpClient.BaseAddress}Name");
            var name = (Name)result.Value;
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(name, new JsonSerializerOptions { WriteIndented = true }));
        }

    }
}
