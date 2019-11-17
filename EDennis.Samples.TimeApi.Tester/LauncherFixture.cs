using EDennis.Samples.Utils;
using System;
using L = EDennis.Samples.TimeApi.Launcher;

namespace EDennis.Samples.TimeApi.Tester {
    public class LauncherFixture : AbstractLauncherFixture {
        public override int Port { get; } = 6501;
        public override Action<string[]> Main { get; } = L.Program.Main;
    }
}
