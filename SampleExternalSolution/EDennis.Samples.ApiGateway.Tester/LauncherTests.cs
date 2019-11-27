using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using G = EDennis.Samples.ApiGateway.Lib;
using L = EDennis.Samples.ApiGateway.Launcher;


namespace EDennis.Samples.ApiGateway.Tester {
    public class LauncherTests : UnitTestBase<G.Program,L.Program> {

        public LauncherTests(LauncherFixture<G.Program,L.Program> fixture,
            ITestOutputHelper output) : base(fixture, output) {
        }


        [Theory]
        [MemberData(nameof(Data))]
        public void GetTime(int idx) {
            var result = HttpClient.Get<Time>($"Time");
            var time = (Time)result.Value;
            if (time == null)
                throw new ApplicationException($"Cannot retrieve time from endpoint: {HttpClient.BaseAddress}Time");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(time, new JsonSerializerOptions { WriteIndented = true }));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetLocation(int idx) {
            var result = HttpClient.Get<Location>($"Location/{idx}");
            var location = (Location)result.Value;
            if (location == null)
                throw new ApplicationException($"Cannot retrieve location from endpoint: {HttpClient.BaseAddress}Location/{idx}");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(location, new JsonSerializerOptions { WriteIndented = true }));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetQuote(int idx) {
            var result = HttpClient.Get<Quote>($"Quote/{idx}");
            var quote = (Quote)result.Value;
            if (quote == null)
                throw new ApplicationException($"Cannot retrieve quote from endpoint: {HttpClient.BaseAddress}Quote/{idx}");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(quote, new JsonSerializerOptions { WriteIndented = true }));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetName(int idx) {
            var result = HttpClient.Get<Name>($"Name/{idx}");
            var name = (Name)result.Value;
            if (name == null)
                throw new ApplicationException($"Cannot retrieve name from endpoint: {HttpClient.BaseAddress}Name/{idx}");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(name, new JsonSerializerOptions { WriteIndented = true }));
        }

    }
}
