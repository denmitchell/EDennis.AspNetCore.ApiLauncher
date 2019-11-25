using EDennis.AspNetCore.Base.Testing;
using System;

namespace EDennis.Samples.NameApi.Scaffolded.Tester {
    public class TestApis : TestApisBase {
        public override Type[] StartupTypes => 
            new Type[] { 
                typeof(TimeApi.Scaffolded.Startup),
                typeof(NameApi.Scaffolded.Startup)
            };
    }
}
