using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using T = EDennis.Samples.TimeApi;
using L = EDennis.Samples.TimeApi.Launcher;
using System;

namespace EDennis.Samples.TimeApi.Tester {
    public class SampleUnitTest : UnitTestBase<T.Program,L.Program> {

        public SampleUnitTest(LauncherFixture<T.Program,L.Program> fixture, 
            ITestOutputHelper output) : base(fixture, output) {
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void GetTime(int idx) {
            var canPing = HttpClient.Ping();
            var result = HttpClient.Get<Time>("time");
            var time = (Time)result.Value;
            if (time == null)
                throw new ApplicationException($"Cannot retrieve time from endpoint: {HttpClient.BaseAddress}Time");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(time, new JsonSerializerOptions { WriteIndented = true }));
        }

    }
}
