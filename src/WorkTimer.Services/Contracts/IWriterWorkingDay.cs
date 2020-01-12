using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWriterWorkingDay : IDbWriter<WorkingDay> {
        Task<WorkingDay> Insert(DateTime dateTime);
    }
}
