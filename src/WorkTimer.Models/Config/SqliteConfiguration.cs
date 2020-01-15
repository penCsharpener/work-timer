using System;
using System.Collections.Generic;
using System.Text;

namespace WorkTimer.Config {
    public class SqliteConfiguration {
        public string PathToDatabase { get; set; } = "%appdata%\\WorkTimer";
        public string DatabaseFileName { get; set; } = "worktimer.db";
        public string DatabasePassword { get; set; } = "";

    }
}
