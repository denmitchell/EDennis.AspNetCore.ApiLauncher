using EDennis.AspNetCore.Base.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace EDennis.Samples.ApiGateway.Tester {
    public class TestApis : TestApisBase {
        public override Dictionary<string,Type> EntryPoints => 
            new Dictionary<string, Type> {
                {"TimeApi", typeof(TimeApi.Program) },
                {"QuoteApi", typeof(QuoteApi.Program) },
                {"NameApi", typeof(NameApi.Program) },
                {"LocationApi", typeof(LocationApi.Program) },
                {"ApiGateway", typeof(Program) }
            };
    }
}
