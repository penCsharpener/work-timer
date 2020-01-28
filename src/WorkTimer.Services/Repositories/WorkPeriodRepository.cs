using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
        private const string OrderByDescStartTime = "ORDER BY StartTime DESC;";

        public WorkPeriodRepository(IDatabaseConnection<SQLiteConnection> conService) {
            _con = conService.Get();
        }

        public async Task<IEnumerable<WorkPeriod>> FindByDate(DateTime date) {
            var p = new {
                date
            };
            var list = await _con.QueryAsync<WorkPeriodRaw>(SelectAll + WhereDate + OrderByDescStartTime, p);
            return list.FromRaw();
        }

        public async Task<IEnumerable<WorkPeriod>> GetAll() {
            var list = await _con.GetAllAsync<WorkPeriodRaw>();
            return list.FromRaw();
        }

        public async Task<IEnumerable<WorkPeriod>> GetIncomplete() {
            var list = await _con.QueryAsync<WorkPeriodRaw>(SelectAll + WhereIncomplete + OrderByDescStartTime);
            return list.FromRaw();
        }

        public async Task<WorkPeriod> FindById(int id) {
            var item = await _con.QueryFirstOrDefaultAsync<WorkPeriodRaw>(SelectAll + WhereById + ";", new { id });
            return item.FromRaw();
        }
    }
}
