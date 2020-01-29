using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Data.SQLite;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Repositories {
    public class WorkPeriodWriter : IWorkPeriodWriter {
        private readonly SQLiteConnection _con;

        public WorkPeriodWriter(IDatabaseConnection<SQLiteConnection> conService) {
            _con = conService.Get();
        }

        private const string DeleteQuery = "DELETE FROM `WorkPeriods` WHERE `_rowid_` IN ('@id');";
        private const string UpdateQuery = "UPDATE `WorkPeriods` SET `Comment`=? WHERE _rowid_='@id';";
        private const string FullUpdateQuery = "UPDATE `WorkPeriods` SET `StartTime`=@startTime, `EndTime`=@endTime, `IsBreak`=@isbreak, `Comment`=@comment WHERE _rowid_=@id;";
        private const string InsertQuery = "INSERT INTO `WorkPeriods`(`StartTime`,`EndTime`,`IsBreak`,`Comment`) VALUES (@StartTime, @EndTime, @IsBreak, @Comment);";

        public async Task Delete(WorkPeriod item) {
            await _con.DeleteAsync(item);
        }

        public async Task<WorkPeriod> Insert(WorkPeriod item) {
            var id = await _con.ExecuteAsync(InsertQuery, item.ToRaw());
            item.Id = id;
            return item;
        }

        public async Task<WorkPeriod> Update(WorkPeriod item, string sql) {
            var s = await _con.UpdateAsync(item);
            return item;
        }

        public async Task<WorkPeriod> Update(int id, DateTime startTime, DateTime? endTime, bool isBreak, string? comment) {
            var p = new {
                id,
                startTime = startTime.Ticks,
                endTime = endTime == null ? default(double?) : endTime.Value.Ticks,
                isBreak = isBreak ? 1 : 0,
                comment
            };
            var result = await _con.ExecuteAsync(FullUpdateQuery, p);
            return new WorkPeriod() {
                Id = id,
                StartTime = startTime,
                EndTime = endTime,
                IsBreak = isBreak,
                Comment = comment
            };
        }
    }
}
