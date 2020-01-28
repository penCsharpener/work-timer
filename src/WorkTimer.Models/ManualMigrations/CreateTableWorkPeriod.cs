namespace WorkTimer.ManualMigrations {
    public static class CreateTableWorkPeriod {
        public static string GetCreateTableWorkPeriod() {
            return "CREATE TABLE IF NOT EXISTS `WorkPeriods` ( " +
                       "`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                       "`StartTime` REAL NOT NULL, " +
                       "`EndTime` REAL, " +
                       "`IsBreak` INTEGER NOT NULL, " +
                       "`Comment` TEXT," +
                       "`ExpectedHours` INTEGER NOT NULL )";
        }
    }
}
