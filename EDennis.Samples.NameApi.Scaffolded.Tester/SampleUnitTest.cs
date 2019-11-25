using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.NameApi.Scaffolded.Tester {
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
        public void TestTimeApi() {

            var timeClient = _factory.CreateClient["TimeApi"]();
            var timeResult = timeClient.Get<Time>("Time");
            var time = (Time)timeResult.Value;
            if (time == null)
                throw new ApplicationException($"Cannot retrieve time from endpoint: {timeClient.BaseAddress}Time");

        }


        [Fact]
        public void TestNameApi() {

            var nameClient = _factory.CreateClient["NameApi"]();
            var nameResult = nameClient.Get<List<Name>>($"Name");
            var names = (List<Name>)nameResult.Value;
            if (names == null)
                throw new ApplicationException($"Cannot retrieve names from endpoint: {nameClient.BaseAddress}Name");
        }

    }
}
