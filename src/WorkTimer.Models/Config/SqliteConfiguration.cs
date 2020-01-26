namespace WorkTimer.Config {
    public class SqliteConfiguration {
        public string PathToDatabase { get; set; } = "%appdata%\\WorkTimer";
        public string DatabaseFileName { get; set; } = "worktimer.db";

    }
}
