﻿using EDennis.AspNetCore.Base.Testing;
using System;

namespace EDennis.Samples.TimeApi.Scaffolded.Tester {
    public class TestApis : TestApisBase {
        public override Type[] StartupTypes => 
            new Type[] { 
                typeof(TimeApi.Scaffolded.Startup)
            };
    }
}