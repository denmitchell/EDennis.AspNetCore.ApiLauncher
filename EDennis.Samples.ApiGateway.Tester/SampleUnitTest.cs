using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using G = EDennis.Samples.ApiGateway;
using L = EDennis.Samples.ApiGateway.Launcher;


namespace EDennis.Samples.ApiGateway.Tester {
    public class SampleUnitTest : UnitTestBase<G.Program,L.Program> {

        public SampleUnitTest(LauncherFixture<G.Program,L.Program> fixture,
            ITestOutputHelper output) : base(fixture, output) {
        }


        [Theory]
        [MemberData(nameof(Data))]
        public void GetTime(int idx) {
            var result = HttpClient.Get<Time>($"{HttpClient.BaseAddress}Time");
            var time = (Time)result.Value;
            if (time == null)
                throw new ApplicationException($"Cannot retrieve time from endpoint: {HttpClient.BaseAddress}Time");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(time, new JsonSerializerOptions { WriteIndented = true }));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetLocation(int idx) {
            var result = HttpClient.Get<Location>($"{HttpClient.BaseAddress}Location");
            var location = (Location)result.Value;
            if (location == null)
                throw new ApplicationException($"Cannot retrieve location from endpoint: {HttpClient.BaseAddress}Location");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(location, new JsonSerializerOptions { WriteIndented = true }));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetQuote(int idx) {
            var result = HttpClient.Get<Quote>($"{HttpClient.BaseAddress}Quote");
            var quote = (Quote)result.Value;
            if (quote == null)
                throw new ApplicationException($"Cannot retrieve quote from endpoint: {HttpClient.BaseAddress}Quote");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(quote, new JsonSerializerOptions { WriteIndented = true }));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetName(int idx) {
            var result = HttpClient.Get<Name>($"{HttpClient.BaseAddress}Name");
            var name = (Name)result.Value;
            if (name == null)
                throw new ApplicationException($"Cannot retrieve name from endpoint: {HttpClient.BaseAddress}Name");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(name, new JsonSerializerOptions { WriteIndented = true }));
        }

    }
}
