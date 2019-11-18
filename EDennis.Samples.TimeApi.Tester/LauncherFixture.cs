using EDennis.Samples.Utils;
using System;
using L = EDennis.Samples.TimeApi.Launcher;

namespace EDennis.Samples.TimeApi.Tester {
    public class LauncherFixture : AbstractLauncherFixture {
        public override int EntryPointPort { get; } = 6501;
        public override Action<string[]> LauncherMain { get; } = L.Program.Main;
    }
}
