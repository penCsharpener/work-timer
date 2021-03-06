﻿using System;
using System.Threading.Tasks;
using WorkTimer.Models;

namespace WorkTimer.Contracts {
    public interface IWriterWorkPeriod : IDbWriter<WorkPeriod> {
        Task<WorkPeriod> Insert(DateTime dateTime, string? comment = null);
        Task<WorkPeriod> UpdateEndTime(int id, DateTime endTime);
    }
}
