using Microsoft.Extensions.Options;
using System;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using WorkTimer.Config;
using WorkTimer.Contracts;

namespace WorkTimer.Repositories {
    public class DbInitService : IDbInitService {
        private readonly IOptions<SqliteConfiguration> _options;

        public string ConnectionString { get; }

        public DbInitService(IOptions<SqliteConfiguration> options) {
            _options = options;
            ConnectionString = _options.Value.ToConnectionString();

        }

        public async Task InitializeDatabase() {
            if (!Directory.Exists(Environment.ExpandEnvironmentVariables(_options.Value.PathToDatabase))) {
                Directory.CreateDirectory(Environment.ExpandEnvironmentVariables(_options.Value.PathToDatabase));
            }
            if (!File.Exists(Environment.ExpandEnvironmentVariables(_options.Value.DatabaseFileName))) {
                SQLiteConnection.CreateFile(Path.Combine(Environment.ExpandEnvironmentVariables(_options.Value.PathToDatabase), _options.Value.DatabaseFileName));
                using (var con = new SQLiteConnection(ConnectionString)) {
                    using (var cmd = new SQLiteCommand(con)) {
                        await con.OpenAsync();
                        cmd.CommandText = @"CREATE TABLE `WorkPeriod` (
                                                `Id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                                `StartTime`	REAL NOT NULL,
                                                `EndTime`	REAL,
                                                `IsBreak`	INTEGER NOT NULL,
                                                `Comment`	TEXT
                                            );";
                        await cmd.ExecuteNonQueryAsync();

                        await con.CloseAsync();
                    }
                }
            }
        }
    }
}
