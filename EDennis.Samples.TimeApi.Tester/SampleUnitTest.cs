using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.TimeApi.Tester {
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

    }
}
