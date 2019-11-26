using EDennis.Samples.SharedModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace EDennis.AspNetCore.Base.Web {
    public interface IProgram {
        Api Api { get; }
        IConfiguration Configuration { get; }
        IHostBuilder CreateHostBuilder(string[] args);
        IProgram Run(string[] args);
    }
}