using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.QuoteApi.Tester {
    public class SampleUnitTest : UnitTestBase<Program> {

        public SampleUnitTest(LauncherFixture<Program> fixture,
            ITestOutputHelper output) : base(fixture, output) {
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetQuote(int idx) {
            var result = HttpClient.Get<Quote>($"{HttpClient.BaseAddress}Quote");
            var quote = (Quote)result.Value;
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(quote, new JsonSerializerOptions { WriteIndented = true }));
        }

    }
}
