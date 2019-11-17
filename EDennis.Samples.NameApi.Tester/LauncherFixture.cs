using EDennis.Samples.Utils;
using System;
using L = EDennis.Samples.NameApi.Launcher;

namespace EDennis.Samples.NameApi.Tester {
    public class LauncherFixture : AbstractLauncherFixture {
        public override int Port { get; } = 7501;
        public override Action<string[]> Main { get; } = L.Program.Main;
    }
}
