using EDennis.AspNetCore.Base.Testing;
using Microsoft.Extensions.Configuration;
using System;

namespace EDennis.Samples.NameApi.Tester {
    public class TestApis : TestApisBase {
        public override Type[] EntryPoints => 
            new Type[] { 
                typeof(TimeApi.Program),
                typeof(Program)
            };

        public override IConfiguration Configuration =>
            new Lib.Program().Configuration;

    }
}
