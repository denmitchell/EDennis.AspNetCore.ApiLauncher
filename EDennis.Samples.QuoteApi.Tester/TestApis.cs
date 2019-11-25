using EDennis.AspNetCore.Base.Testing;
using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.Samples.QuoteApi.Tester {
    public class TestApis : TestApisBase {
        public override Type[] StartupTypes => 
            new Type[] { 
                typeof(TimeApi.Program),
                typeof(Program)
            };

        public override IConfiguration Configuration =>
            new Lib.Program().Configuration;

    }
}
