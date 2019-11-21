using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.NameApi.Tester {
    public class SampleUnitTest : UnitTestBase<Program> {

        public SampleUnitTest(LauncherFixture<Program> fixture,
            ITestOutputHelper output) : base(fixture, output) {
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
