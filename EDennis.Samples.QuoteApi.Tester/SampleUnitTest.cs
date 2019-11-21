using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using L = EDennis.Samples.QuoteApi.Launcher;
using Q = EDennis.Samples.QuoteApi;


namespace EDennis.Samples.QuoteApi.Tester {
    public class SampleUnitTest : UnitTestBase<Q.Program,L.Program> {

        public SampleUnitTest(LauncherFixture<Q.Program, L.Program> fixture,
            ITestOutputHelper output) : base(fixture, output) {
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

    }
}
