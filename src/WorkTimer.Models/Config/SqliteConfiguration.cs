using System;
using System.IO;

namespace WorkTimer.Config {
    public class SqliteConfiguration {
        public string PathToDatabase { get; set; } = "%appdata%\\WorkTimer";
        public string DatabaseFileName { get; set; } = "worktimer.db";

        public string DatabaseFullPath => Environment.ExpandEnvironmentVariables(Path.Combine(PathToDatabase, DatabaseFileName));

    }
}
