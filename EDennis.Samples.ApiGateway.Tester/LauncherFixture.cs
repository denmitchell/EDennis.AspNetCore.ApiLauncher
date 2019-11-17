using EDennis.Samples.Utils;
using System;
using L = EDennis.Samples.ApiGateway.Launcher;

namespace EDennis.Samples.ApiGateway.Tester {
    public class LauncherFixture : AbstractLauncherFixture {
        public override int Port { get; } = 9901;
        public override Action<string[]> Main { get; } = L.Program.Main;
    }
}
