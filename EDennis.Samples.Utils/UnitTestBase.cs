using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.SharedModel;
using EDennis.Samples.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Base.Testing {
    public class UnitTestBase<TProgram> : IClassFixture<LauncherFixture<TProgram>> 
        where TProgram : IProgram, new(){

        public ITestOutputHelper Output { get; }
        public HttpClient HttpClient { get; }

        public UnitTestBase(LauncherFixture<TProgram> fixture, ITestOutputHelper output) {
            HttpClient = fixture.HttpClient;
            Output = output;
        }

        public static IEnumerable<object[]> Data =>
                Enumerable.Range(1, 10).Select(i => new object[] { i }).ToArray();
    }
}
