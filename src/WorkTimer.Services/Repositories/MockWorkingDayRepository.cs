using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Contracts;
using WorkTimer.Models;

namespace WorkTimer.Repositories {
    public class MockWorkingDayRepository : IWorkingDayRepository {
        public async Task<IEnumerable<WorkingDay>> GetAll() {
            await Task.Delay(0);
            return new List<WorkingDay>() {
                new WorkingDay() { Id = 1, Date = DateTime.Now.AddDays(-5) },
                new WorkingDay() { Id = 2, Date = DateTime.Now.AddDays(-4) },
                new WorkingDay() { Id = 3, Date = DateTime.Now.AddDays(-3) },
                new WorkingDay() { Id = 4, Date = DateTime.Now.AddDays(-2) },
                new WorkingDay() { Id = 5, Date = DateTime.Now.AddDays(-1) },
            };
        }
    }
}
