namespace WorkTimer.ManualMigrations {
    public static class CreateTableWorkPeriod {
        public static string GetCreateTableWorkPeriod() {
            return "CREATE TABLE IF NOT EXISTS `WorkPeriods` ( " +
                       "`Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                       "`StartTime` TEXT NOT NULL, " +
                       "`EndTime` TEXT, " +
                       "`IsBreak` INTEGER NOT NULL, " +
                       "`Comment` TEXT );";
        }

        public static string CreateIndeces() {
            return "CREATE UNIQUE INDEX `workperiods_id_asc` ON `WorkPeriods` ( `Id`\tASC );" +
                   "CREATE INDEX `workperiods_starttime_asc` ON `WorkPeriods` ( `StartTime`\tASC );" +
                   "CREATE INDEX `workperiods_starttime_desc` ON `WorkPeriods` ( `StartTime`\tDESC );" +
                   "CREATE INDEX `workperiods_endtime_asc` ON `WorkPeriods` ( `EndTime`\tASC ); " +
                   "CREATE INDEX `workperiods_endtime_desc` ON `WorkPeriods` ( `EndTime`\tDESC ); ";
        }
    }
}
