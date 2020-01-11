using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimer.Contracts {
    public interface IStartTracking {
        Task StartTracking(DateTime dateTime);
    }
}
