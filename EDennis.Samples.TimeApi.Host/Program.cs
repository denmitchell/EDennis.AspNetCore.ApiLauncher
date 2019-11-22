using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.TimeApi.App {
    public class Program {
        public static void Main(string[] args) {
            new T.Program().CreateHostBuilder(args).Build().Run();
        }
    }
}
