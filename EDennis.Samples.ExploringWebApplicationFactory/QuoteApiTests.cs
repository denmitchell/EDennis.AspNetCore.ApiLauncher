using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;
using Q = EDennis.Samples.QuoteApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.ExploringWebApplicationFactory {
    public class QuoteApiTests : 
        IClassFixture<TimeApiWebApplicationFactory<T.Program>>,
        IClassFixture<QuoteApiWebApplicationFactory<Q.Program>>{


        private readonly TimeApiWebApplicationFactory<T.Program> _timeFactory;
        private readonly QuoteApiWebApplicationFactory<Q.Program> _quoteFactory;

        private readonly ITestOutputHelper _output;
        public QuoteApiTests(
            TimeApiWebApplicationFactory<T.Program> timeFactory,
            QuoteApiWebApplicationFactory<Q.Program> quoteFactory,
            ITestOutputHelper output) {
            _timeFactory = timeFactory;
            _quoteFactory = quoteFactory;
            _output = output;
        }


        [Fact]
        public void Test1() {

            //Explicitly set the server base addresses (which default to "http://localhost/")
            //Note: the servers don't appear to be created until you attempt to access them.
            var timeServer = _timeFactory.Server;
            var quoteServer = _quoteFactory.Server;

            //set the URIs equal to the URIs from the Api configuration
            var timeUri = new Uri(_timeFactory.Api.MainAddress);
            var quoteUri = new Uri(_quoteFactory.Api.MainAddress);

            _timeFactory.Server.BaseAddress = timeUri;
            _quoteFactory.Server.BaseAddress = quoteUri;

            //Query the time API.  This works just fine because you are using
            //  the test server's own client to access the test server 
            var timeClient = _timeFactory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = timeUri });
            var timeResult = timeClient.Get<Time>("Time");
            var time = (Time)timeResult.Value;
            if (time == null)
                throw new ApplicationException($"Cannot retrieve time from endpoint: {timeClient.BaseAddress}Time");

            //Try to retrieve time from the same URI, but from a regular client.
            //This fails because the test server cannot be accessed outside of its own client
            using var timeClient2 = new HttpClient { BaseAddress = timeUri };
            Assert.Throws<AggregateException>(() => {
                var timeResult2 = timeClient2.GetAsync($"{timeClient2.BaseAddress}Time").Result;
            });

            //Try to retreive quotes, which fails because it cannot contact the TimeApi server
            var quoteClient = _quoteFactory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = quoteUri });
            var quoteResult = quoteClient.Get<List<Quote>>($"Quote");
            Assert.Throws<InvalidCastException>(() => {
                var quotes = (List<Quote>)quoteResult.Value;
            });

        }
    }
}
