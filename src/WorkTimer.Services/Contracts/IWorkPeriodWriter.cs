using System;
using System.Threading.Tasks;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWorkPeriodWriter : IDbWriter<WorkPeriod> {
        Task<WorkPeriod> Update(int id, DateTime startTime, DateTime? endTime, bool isBreak, string? comment);
    }
}
