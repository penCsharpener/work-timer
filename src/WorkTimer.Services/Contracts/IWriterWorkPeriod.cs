using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWriterWorkPeriod : IDbWriter<WorkPeriod> {
        Task<WorkPeriod> Insert(DateTime dateTime, string? comment = null);
        Task<WorkPeriod> Insert(int workDayId, DateTime dateTime, string? comment = null);
        Task<WorkPeriod> UpdateEndTime(int id, DateTime endTime);
    }
}
