using System;
using System.Collections.Generic;
using System.Text;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWriterWorkPeriod : IDbWriter<WorkPeriod> {
    }
}
