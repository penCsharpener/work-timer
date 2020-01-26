using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWorkingDayRepository {

        Task<IEnumerable<WorkingDay>> GetAll();
        /// <summary>
        /// returns all existing data for the day, with WorkPeriods and WorkBreaks
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        Task<WorkingDay?> FindByDate(DateTime dateTime);

    }
}
