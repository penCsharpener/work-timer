using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWriterWorkBreak : IDbWriter<WorkBreak> {
        Task<WorkBreak> Insert(DateTime dateTime, string? comment = null);
    }
}
