using EDennis.Samples.SharedModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace EDennis.AspNetCore.Base.Web {
    public interface IProgram {
        Api Api { get; }
        string ApisConfigurationSection { get; }
        IConfiguration Configuration { get; }
        Type Startup { get; }
        bool UsesEmbeddedConfigurationFiles { get; }
        bool UsesLauncherConfigurationFile { get; }

        IHostBuilder CreateHostBuilder(string[] args);
        IProgram Run(string[] args);
    }
}