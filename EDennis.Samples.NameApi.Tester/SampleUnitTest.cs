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
        [MemberData(nameof(Data))]
        public void GetName(int idx) {
            var result = HttpClient.Get<Name>($"{HttpClient.BaseAddress}Name");
            var name = (Name)result.Value;
            if (name == null)
                throw new ApplicationException($"Cannot retrieve name from endpoint: {HttpClient.BaseAddress}Name");
            Output.WriteLine($"{idx}: " + JsonSerializer.Serialize(name, new JsonSerializerOptions { WriteIndented = true }));
        }


    }
}
