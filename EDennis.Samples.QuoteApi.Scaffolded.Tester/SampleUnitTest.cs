using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.QuoteApi.Scaffolded.Tester {
    public class SampleUnitTest : 
        IClassFixture<TestApis>{


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public SampleUnitTest(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void Test1() {

            var timeClient = _factory.CreateClient["TimeApi"]();
            var timeResult = timeClient.Get<Time>("Time");
            var time = (Time)timeResult.Value;
            if (time == null)
                throw new ApplicationException($"Cannot retrieve time from endpoint: {timeClient.BaseAddress}Time");


            var quoteClient = _factory.CreateClient["QuoteApi"]();
            var quoteResult = quoteClient.Get<List<Quote>>($"Quote");
            var quotes = (List<Quote>)quoteResult.Value;
            if (quotes == null)
                throw new ApplicationException($"Cannot retrieve quotes from endpoint: {quoteClient.BaseAddress}Quote");


        }
    }
}
