using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Repositories {
    public class MockWorkPeriodRepository : IWorkPeriodRepository {

        public static List<WorkPeriod> Data = new List<WorkPeriod>() {
                new WorkPeriod() { Id = 1, StartTime = new DateTime(2020,1,6,7, 7, 0), EndTime = new DateTime(2020,1,6,12, 2, 0) },
                new WorkPeriod() { Id = 2, StartTime = new DateTime(2020,1,6,12, 33, 0), EndTime = new DateTime(2020,1,6,16, 35, 0) },
                new WorkPeriod() { Id = 3, StartTime = new DateTime(2020,1,7,6, 45, 0), EndTime = new DateTime(2020,1,7,11, 56, 0) },
                new WorkPeriod() { Id = 4, StartTime = new DateTime(2020,1,7,12, 24, 0), EndTime = new DateTime(2020,1,7,15, 29, 0) },
                new WorkPeriod() { Id = 5, StartTime = new DateTime(2020,1,8,6, 55, 0), EndTime = new DateTime(2020,1,8,12, 1, 0) },
                new WorkPeriod() { Id = 6, StartTime = new DateTime(2020,1,8,12, 39, 0), EndTime = new DateTime(2020,1,8,16, 33, 0) },
                new WorkPeriod() { Id = 7, StartTime = new DateTime(2020,1,9,6, 47, 0), EndTime = new DateTime(2020,1,9,12, 0, 0) },
                new WorkPeriod() { Id = 8, StartTime = new DateTime(2020,1,9,12, 29, 0), EndTime = new DateTime(2020,1,9,16, 56, 0) },
                new WorkPeriod() { Id = 9, StartTime = new DateTime(2020,1,10,8, 52, 0), EndTime = new DateTime(2020,1,10,11, 38, 0) },
                new WorkPeriod() { Id = 10, StartTime = new DateTime(2020,1,10,12, 31, 0), EndTime = new DateTime(2020,1,10,16, 36, 0) },
                new WorkPeriod() { Id = 11, StartTime = new DateTime(2020, 1, 10, 14, 11, 14), EndTime = new DateTime(2020, 1, 10, 14, 13, 55), IsBreak = true, Comment = "stretching my legs" },
            };

        public MockWorkPeriodRepository() {

        }

        public Task<IEnumerable<WorkPeriod>> FindByDate(DateTime date) {
            return Task.FromResult(Data.Where(x => x.Date == date.Date));
        }

        public Task<WorkPeriod> FindById(int id) {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public async Task<IEnumerable<WorkPeriod>> GetAll() {
            await Task.Delay(0);
            return Data;
        }

        public async Task<IEnumerable<WorkPeriod>> GetIncomplete() {
            await Task.Delay(0);
            return Data.Where(x => !x.EndTime.HasValue);
        }
    }
}
