using EDennis.Samples.SharedModel;
using T = EDennis.Samples.TimeApi.Scaffolded;
using Q = EDennis.Samples.QuoteApi.Scaffolded;
using EDennis.Samples.Utils;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.Samples.ExploringWebApplicationFactory {
    public class QuoteApiTests2 : 
        IClassFixture<TimeApiWebApplicationFactory<T.Startup>>,
        IClassFixture<QuoteApiWebApplicationFactory2<Q.Startup>>{


        private readonly TimeApiWebApplicationFactory<T.Startup> _timeFactory;
        private readonly QuoteApiWebApplicationFactory2<Q.Startup> _quoteFactory;

        private readonly ITestOutputHelper _output;
        public QuoteApiTests2(
            TimeApiWebApplicationFactory<T.Startup> timeFactory,
            QuoteApiWebApplicationFactory2<Q.Startup> quoteFactory,
            ITestOutputHelper output) {
            _timeFactory = timeFactory;
            _quoteFactory = quoteFactory;
            _quoteFactory.TimeApiWebApplicationFactory = _timeFactory;
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


            //Try to retreive quotes, which should work because we are injecting in a 
            //  TestHttpClientFactory, which uses the WebApplicationFactory to create the
            //  HttpClient that the QuoteApi uses to query the TimeApi.
            var quoteClient = _quoteFactory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = quoteUri });
            var quoteResult = quoteClient.Get<List<Quote>>($"Quote");
            var quotes = (List<Quote>)quoteResult.Value;
            if (quotes == null)
                throw new ApplicationException($"Cannot retrieve quotes from endpoint: {quoteClient.BaseAddress}Quote");

        }
    }
}
