using EDennis.Samples.Utils;
using System;
using L = EDennis.Samples.LocationApi.Launcher;

namespace EDennis.Samples.LocationApi.Tester {
    public class LauncherFixture : AbstractLauncherFixture {
        public override int Port { get; } = 7501;
        public override Action<string[]> Main { get; } = L.Program.Main;
    }
}
