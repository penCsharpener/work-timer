using Microsoft.Extensions.Options;
using System;
using System.Data.SQLite;
using WorkTimer.Config;
using WorkTimer.Contracts;

namespace WorkTimer.Services {
    public class SqliteDatabaseConnectionService : IDatabaseConnection<SQLiteConnection>, IDisposable {
        private readonly IOptions<SqliteConfiguration> _options;
        private readonly SQLiteConnection _con;

        public SqliteDatabaseConnectionService(IOptions<SqliteConfiguration> options) {
            _options = options;
            _con = new SQLiteConnection(_options.Value.ToConnectionString());
        }

        public SQLiteConnection Get() {
            return _con;
        }

        public void Dispose() {
            _con.Dispose();
        }
    }
}
