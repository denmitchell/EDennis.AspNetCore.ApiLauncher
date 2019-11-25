using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using L = EDennis.Samples.LocationApi.Lib;
using LL = EDennis.Samples.LocationApi.Launcher;


namespace EDennis.Samples.LocationApi.Tester {
    public class LauncherTests : UnitTestBase<L.Program,LL.Program> {

        public LauncherTests(LauncherFixture<L.Program,LL.Program> fixture,
            ITestOutputHelper output) : base(fixture, output) {
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

    }
}
