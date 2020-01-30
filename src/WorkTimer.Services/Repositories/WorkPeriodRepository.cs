using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Repositories {
    public class WorkPeriodRepository : IWorkPeriodRepository {
        private readonly SQLiteConnection _con;

        private const string SelectAll = "SELECT * FROM WorkPeriods ";
        private const string WhereDate = "WHERE Date(StartTime) = @date ";
        private const string WhereIncomplete = "WHERE EndTime IS NULL ";
        private const string WhereById = "Where Id = @id ";
        private const string OrderByDescStartTime = "ORDER BY StartTime DESC ";
        private const string LimitClause = "LIMIT @limit;";

        public WorkPeriodRepository(IDatabaseConnection<SQLiteConnection> conService) {
            _con = conService.Get();
        }

        public async Task<IEnumerable<WorkPeriod>> FindByDate(DateTime date) {
            var list = await GetAll();
            return list.Where(x => x.StartTime.Date == date.Date).ToList();
        }

        public async Task<IEnumerable<WorkPeriod>> GetAll() {
            var list = await _con.GetAllAsync<WorkPeriodRaw>();
            return list.FromRaw().OrderByDescending(x => x.StartTime);
        }

        public async Task<IEnumerable<WorkPeriod>> GetIncomplete() {
            var list = await _con.QueryAsync<WorkPeriodRaw>(SelectAll + WhereIncomplete + OrderByDescStartTime + ";");
            return list.FromRaw();
        }

        public async Task<WorkPeriod> FindById(int id) {
            var item = await _con.QueryFirstOrDefaultAsync<WorkPeriodRaw>(SelectAll + WhereById + ";", new { id });
            return item.FromRaw();
        }

        public async Task<IEnumerable<WorkPeriod>> MostRecent(int limit) {
            var items = await _con.QueryAsync<WorkPeriodRaw>(SelectAll + OrderByDescStartTime + LimitClause, new { limit });
            return items.FromRaw();
        }
    }
}
