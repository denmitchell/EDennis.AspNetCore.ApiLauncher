using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.LocationApi.Tester {
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
    }
}
