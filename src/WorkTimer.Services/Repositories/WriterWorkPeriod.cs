using Dapper;
using System;
using System.Data.SQLite;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Repositories {
    public class WriterWorkPeriod : IWriterWorkPeriod {
        private readonly IWorkPeriodWriter _writer;
        private readonly IWorkPeriodRepository _repo;
        private readonly SQLiteConnection _con;

        public WriterWorkPeriod(IWorkPeriodWriter writer,
                                IWorkPeriodRepository repo,
                                IDatabaseConnection<SQLiteConnection> conService) {
            _writer = writer;
            _repo = repo;
            _con = conService.Get();
        }

        public async Task Delete(WorkPeriod item) {
            await _writer.Delete(item);
        }

        public async Task<WorkPeriod> Insert(DateTime dateTime, string? comment = null) {
            return await _writer.Insert(new WorkPeriod() {
                StartTime = dateTime,
                Comment = comment,
            });
        }

        public async Task<WorkPeriod> Insert(WorkPeriod item) {
            return await _writer.Insert(item);
        }

        public async Task<WorkPeriod> Update(WorkPeriod item, string sql) {
            return await _writer.Update(item, sql);
        }

        private const string UpdateEndTimeQuery = "UPDATE `WorkPeriods` SET `EndTime`=@endTime WHERE _rowid_=@id;";

        public async Task<WorkPeriod> UpdateEndTime(int id, DateTime endTime) {
            var p = new {
                id,
                endTime = endTime == null ? default(string?) : endTime.ToSqlite(),
            };
            var result = await _con.ExecuteAsync(UpdateEndTimeQuery, p);
            return await _repo.FindById(id);
        }
    }
}
