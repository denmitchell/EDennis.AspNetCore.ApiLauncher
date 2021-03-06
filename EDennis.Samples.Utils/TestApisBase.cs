﻿using EDennis.Samples.SharedModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class TestApisBase  : IDisposable{

        public Dictionary<string, Func<HttpClient>> CreateClient { get; }
            = new Dictionary<string, Func<HttpClient>> ();

        protected Dictionary<string, Action> _dispose
            = new Dictionary<string, Action>();

        public abstract Dictionary<string,Type> EntryPoints { get; }
        
        public TestApisBase() {

            //now populate the dictionary with TestApi instances
            foreach (var httpClientName in EntryPoints.Keys) {
                Type[] typeParams = new Type[] { EntryPoints[httpClientName] };
                Type classType = typeof(TestApi<>);
                Type constructedType = classType.MakeGenericType(typeParams);

                var _ = 
                    Activator.CreateInstance(constructedType, 
                        new object[] { CreateClient, _dispose, httpClientName });
            }

        }

        public void Dispose() {
            foreach (var key in _dispose.Keys)
                _dispose[key]();
        }



    }
}
