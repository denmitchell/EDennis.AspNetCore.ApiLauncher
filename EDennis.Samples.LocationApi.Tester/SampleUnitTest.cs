using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using L = EDennis.Samples.LocationApi;
using LL = EDennis.Samples.LocationApi.Launcher;


namespace EDennis.Samples.LocationApi.Tester {
    public class SampleUnitTest : UnitTestBase<L.Program,LL.Program> {

        public SampleUnitTest(LauncherFixture<L.Program,LL.Program> fixture,
            ITestOutputHelper output) : base(fixture, output) {
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

    }
}
