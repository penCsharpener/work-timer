using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWorkingDayRepository {

        Task<IEnumerable<WorkingDay>> GetAll();
        Task<IEnumerable<WorkingDay>> FindByIds(IEnumerable<int> ids);
        Task<IEnumerable<WorkingDay>> GetIncomplete();

    }
}
