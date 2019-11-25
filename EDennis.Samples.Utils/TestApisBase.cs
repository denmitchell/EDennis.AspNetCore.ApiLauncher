using EDennis.Samples.SharedModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using IStartup = EDennis.AspNetCore.Base.Web.IStartup;

namespace EDennis.AspNetCore.Base.Testing {

    public abstract class TestApisBase  : IDisposable{

        public Dictionary<string, Func<HttpClient>> CreateClient { get; }
            = new Dictionary<string, Func<HttpClient>> ();

        protected Dictionary<string, Action> _dispose
            = new Dictionary<string, Action>();

        public abstract Type[] StartupTypes { get; }
        
        public virtual IConfiguration Configuration { 
            get {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.Development.json")
                    .Build();
                return configuration;
            }
        }

        public virtual string ApisConfigurationKey { get; } = "Apis";

        public Apis Apis { get; }

        public TestApisBase() {

            Apis = new Apis();
            Configuration.GetSection(ApisConfigurationKey).Bind(Apis);

            //now populate the dictionary with TestApi instances
            foreach (var type in StartupTypes) {
                Type[] typeParams = new Type[] { type };
                Type classType = typeof(TestApi<>);
                Type constructedType = classType.MakeGenericType(typeParams);

                var testApi = 
                    Activator.CreateInstance(constructedType, 
                        new object[] { CreateClient, _dispose, Configuration, ApisConfigurationKey, Apis });
            }

        }

        public void Dispose() {
            foreach (var key in _dispose.Keys)
                _dispose[key]();
        }



    }
}
