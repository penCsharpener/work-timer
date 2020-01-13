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
        /// <summary>
        /// returns all existing data for the day, with WorkPeriods and WorkBreaks
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        Task<WorkingDay?> FindByDate(DateTime dateTime);
        Task<IEnumerable<WorkingDay>> GetIncomplete();

    }
}
