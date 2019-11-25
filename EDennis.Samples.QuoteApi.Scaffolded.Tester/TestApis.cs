using EDennis.AspNetCore.Base.Testing;
using System;

namespace EDennis.Samples.QuoteApi.Scaffolded.Tester {
    public class TestApis : TestApisBase {
        public override Type[] StartupTypes => 
            new Type[] { 
                typeof(TimeApi.Scaffolded.Startup),
                typeof(QuoteApi.Scaffolded.Startup)
            };
    }
}
