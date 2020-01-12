using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWorkBreakRepository {
        Task<IEnumerable<WorkBreak>> GetAll();
        Task<IEnumerable<WorkBreak>> FindByWorkPeriodIds(IEnumerable<int> workPeriodIds);
        Task<IEnumerable<WorkBreak>> GetIncomplete();
    }
}
