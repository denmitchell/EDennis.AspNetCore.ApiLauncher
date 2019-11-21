using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System;
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
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetQuote(int idx) {
            var result = HttpClient.Get<Quote>($"Quote/{idx}");
            var quote = (Quote)result.Value;
            if (quote == null)
                throw new ApplicationException($"Cannot retrieve quote from endpoint: {HttpClient.BaseAddress}Quote");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(quote, new JsonSerializerOptions { WriteIndented = true }));
        }

    }
}
