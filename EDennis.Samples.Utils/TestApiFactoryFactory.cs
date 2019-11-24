using EDennis.AspNetCore.Base.Web;
using EDennis.Samples.SharedModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.Utils {

    public abstract class TestApiFactoryFactory  : IDisposable{

        public Dictionary<string, TestApiFactory<IStartup>> Factories { get; } 
            = new Dictionary<string, TestApiFactory<IStartup>>();

        public abstract Type[] StartupTypes { get; }
        public abstract Apis Apis { get; }


        public TestApiFactoryFactory() {

            //initial loop to ensure that the dictionary has all of its keys
            foreach(var type in StartupTypes) {
                var api = Apis.FirstOrDefault(a => a.Value.ProjectName == type.Assembly.GetName().Name).Key;
                Factories.Add(api, null);
            }

            //now populate the dictionary with TestApiFactory instances
            foreach (var type in StartupTypes) {
                var api = Apis.FirstOrDefault(a => a.Value.ProjectName == type.Assembly.GetName().Name).Key;
                Type[] typeParams = new Type[] { type };
                Type classType = typeof(TestApiFactory<>);
                Type constructedType = classType.MakeGenericType(typeParams);
                TestApiFactory<IStartup> factory = 
                    (TestApiFactory<IStartup>) Activator.CreateInstance(constructedType, new object[] { Factories });
                Factories[api] = factory;
                //call the server to ensure creation
                var _ = factory.Server;
            }

        }

        public void Dispose() {
            foreach (var key in Factories.Keys)
                Factories[key].Dispose();
        }
    }



}
