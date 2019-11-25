using EDennis.AspNetCore.Base.Testing;
using System;

namespace EDennis.Samples.ApiGateway.Scaffolded.Tester {
    public class TestApis : TestApisBase {
        public override Type[] StartupTypes => 
            new Type[] { 
                typeof(TimeApi.Scaffolded.Startup),
                typeof(QuoteApi.Scaffolded.Startup),
                typeof(NameApi.Scaffolded.Startup),
                typeof(LocationApi.Scaffolded.Startup),
                typeof(ApiGateway.Scaffolded.Startup),
            };
    }
}
