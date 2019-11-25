using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.Samples.TimeApi.Scaffolded.Tester {
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
    }
}
