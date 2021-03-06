﻿using System;
using System.Threading.Tasks;

namespace WorkTimer.Contracts {
    public interface IToggleTracking {

        /// <summary>
        /// Starts tracking for the day if not existent or ends all running tracking.
        /// Stopping the tracking ignores any comment input.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task ToggleTracking(DateTime dateTime, bool isBreak, string? comment = null);

        /// <summary>
        /// Checks with provided date whether there is a WorkingDay item for that date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<bool> TrackingExists(DateTime date);

        /// <summary>
        /// Check with DateTime.Now whether there is a WorkingDay item with the current date
        /// </summary>
        /// <returns></returns>
        Task<bool> TrackingExistsForToday();
    }
}
