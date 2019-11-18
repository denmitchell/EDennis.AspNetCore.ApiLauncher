using EDennis.Samples.Utils;
using System;
using L = EDennis.Samples.QuoteApi.Launcher;

namespace EDennis.Samples.QuoteApi.Tester {
    public class LauncherFixture : AbstractLauncherFixture {
        public override int EntryPointPort { get; } = 7501;
        public override Action<string[]> LauncherMain { get; } = L.Program.Main;
    }
}
