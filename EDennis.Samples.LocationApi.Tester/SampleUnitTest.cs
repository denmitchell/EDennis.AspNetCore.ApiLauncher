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

namespace EDennis.Samples.LocationApi.Tester {
    public class SampleUnitTest : UnitTestBase<Program> {

        public SampleUnitTest(LauncherFixture<Program> fixture,
            ITestOutputHelper output) : base(fixture, output) {
        }


        [Theory]
        [MemberData(nameof(Data))]
        public void GetLocation(int idx) {
            var result = HttpClient.Get<Location>($"{HttpClient.BaseAddress}Location");
            var location = (Location)result.Value;
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(location, new JsonSerializerOptions { WriteIndented = true }));
        }

    }
}
