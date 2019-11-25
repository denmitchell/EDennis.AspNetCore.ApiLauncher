using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.ApiGateway.Scaffolded.Tester {
    public class FactoryTests : 
        IClassFixture<TestApis>{


        private readonly TestApis _factory;

        private readonly ITestOutputHelper _output;
        public FactoryTests(
            TestApis factory,
            ITestOutputHelper output) {
            _factory = factory;
            _output = output;
        }


        [Fact]
        public void TestTimeApi() {

            var timeClient = _factory.CreateClient["TimeApi"]();
            var timeResult = timeClient.Get<Time>("Time");
            var time = (Time)timeResult.Value;
            if (time == null)
                throw new ApplicationException($"Cannot retrieve time from endpoint: {timeClient.BaseAddress}Time");

        }


        [Fact]
        public void TestLocationApi() {

            var locationClient = _factory.CreateClient["LocationApi"]();
            var locationResult = locationClient.Get<List<Location>>($"Location");
            var locations = (List<Location>)locationResult.Value;
            if (locations == null)
                throw new ApplicationException($"Cannot retrieve locations from endpoint: {locationClient.BaseAddress}Location");

        }


        [Fact]
        public void TestApiGateway_Location() {

            var locationClient = _factory.CreateClient["ApiGateway"]();
            var locationResult = locationClient.Get<List<Location>>($"Location");
            var locations = (List<Location>)locationResult.Value;
            if (locations == null)
                throw new ApplicationException($"Cannot retrieve locations from endpoint: {locationClient.BaseAddress}Location");
        }

        [Fact]
        public void TestNameApi() {

            var nameClient = _factory.CreateClient["NameApi"]();
            var nameResult = nameClient.Get<List<Name>>($"Name");
            var names = (List<Name>)nameResult.Value;
            if (names == null)
                throw new ApplicationException($"Cannot retrieve names from endpoint: {nameClient.BaseAddress}Name");

        }


        [Fact]
        public void TestApiGateway_Name() {

            var nameClient = _factory.CreateClient["ApiGateway"]();
            var nameResult = nameClient.Get<List<Name>>($"Name");
            var names = (List<Name>)nameResult.Value;
            if (names == null)
                throw new ApplicationException($"Cannot retrieve names from endpoint: {nameClient.BaseAddress}Name");
        }



        [Fact]
        public void TestQuoteApi() {

            var quoteClient = _factory.CreateClient["QuoteApi"]();
            var quoteResult = quoteClient.Get<List<Quote>>($"Quote");
            var quotes = (List<Quote>)quoteResult.Value;
            if (quotes == null)
                throw new ApplicationException($"Cannot retrieve quotes from endpoint: {quoteClient.BaseAddress}Quote");

        }


        [Fact]
        public void TestApiGateway_Quote() {

            var quoteClient = _factory.CreateClient["ApiGateway"]();
            var quoteResult = quoteClient.Get<List<Quote>>($"Quote");
            var quotes = (List<Quote>)quoteResult.Value;
            if (quotes == null)
                throw new ApplicationException($"Cannot retrieve quotes from endpoint: {quoteClient.BaseAddress}Quote");
        }


    }
}
