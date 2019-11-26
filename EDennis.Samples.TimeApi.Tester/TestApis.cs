using EDennis.AspNetCore.Base.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.TimeApi.Tester {
    public class TestApis : TestApisBase {
        public override Dictionary<string, Type> EntryPoints =>
            new Dictionary<string, Type> {
                {"TimeApi", typeof(TimeApi.Program) }
            };
    }
}
