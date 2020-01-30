using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWorkPeriodRepository {
        Task<IEnumerable<WorkPeriod>> GetAll();
        Task<IEnumerable<WorkPeriod>> FindByDate(DateTime date);
        Task<IEnumerable<WorkPeriod>> GetIncomplete();
        Task<WorkPeriod> FindById(int id);
        Task<IEnumerable<WorkPeriod>> MostRecent(int limit);
    }
}
