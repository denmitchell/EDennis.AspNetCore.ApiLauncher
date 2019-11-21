﻿using EDennis.Samples.Utils;
using Q = EDennis.Samples.QuoteApi;
using T = EDennis.Samples.TimeApi;

namespace EDennis.Samples.QuoteApi.Launcher {
    public class Program {
        public static void Main(string[] args) {
            new T.Program().Run(args);
            new Q.Program().Run(args);
            LauncherUtils.Block(args);
        }

    }
}
