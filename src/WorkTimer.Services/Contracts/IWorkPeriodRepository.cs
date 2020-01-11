using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWorkPeriodRepository {
        Task<IEnumerable<WorkPeriod>> GetAll();
        Task<IEnumerable<WorkPeriod>> FindByWorkingDayIds(IEnumerable<int> workingDayIds);

    }
}
