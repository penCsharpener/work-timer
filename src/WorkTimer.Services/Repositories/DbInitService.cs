using Microsoft.Extensions.Options;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using WorkTimer.Config;
using WorkTimer.Contracts;
using WorkTimer.ManualMigrations;

namespace WorkTimer.Repositories {
    public class DbInitService : IDbInitService {
        private readonly IOptions<SqliteConfiguration> _options;

        public string ConnectionString { get; }

        public DbInitService(IOptions<SqliteConfiguration> options) {
            _options = options;
            ConnectionString = _options.Value.ToConnectionString();

        }

        public async Task InitializeDatabase() {
            var path = Path.GetDirectoryName(_options.Value.DatabaseFullPath);
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            if (!File.Exists(_options.Value.DatabaseFullPath)) {
                SQLiteConnection.CreateFile(_options.Value.DatabaseFullPath);
                using (var con = new SQLiteConnection(ConnectionString)) {
                    using (var cmd = new SQLiteCommand(con)) {
                        await con.OpenAsync();
                        cmd.CommandText = CreateTableWorkPeriod.GetCreateTableWorkPeriod();
                        await cmd.ExecuteNonQueryAsync();

                        await con.CloseAsync();
                    }
                }
            }
        }
    }
}
