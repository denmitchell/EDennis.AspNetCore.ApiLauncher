using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using N = EDennis.Samples.NameApi;
using L = EDennis.Samples.NameApi.Launcher;
using System;

namespace EDennis.Samples.NameApi.Tester {
    public class SampleUnitTest : UnitTestBase<N.Program,L.Program> {

        public SampleUnitTest(LauncherFixture<N.Program,L.Program> fixture,
            ITestOutputHelper output) : base(fixture, output) {
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetName(int idx) {
            var result = HttpClient.Get<Name>($"Name/{idx}");
            var name = (Name)result.Value;
            if (name == null)
                throw new ApplicationException($"Cannot retrieve name from endpoint: {HttpClient.BaseAddress}Name/{idx}");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(name, new JsonSerializerOptions { WriteIndented = true }));
        }


    }
}
