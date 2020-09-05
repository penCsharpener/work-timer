using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimer.Domain.Models;

namespace WorkTimer.MediatR.Responses {
    public class IndexResponse {
        public IList<DisplayWorkDayModel> WorkDays { get; set; }
        public IList<WorkingPeriod> MostRecentWorkPeriods { get; set; }
        public TimeSpan TotalOverHours { get; set; }
        public bool HasOngoingWorkPeriod { get; set; }
        public string RedirectRoute { get; set; }
    }

    public class DisplayWorkDayModel : WorkDay {
        public DisplayWorkDayModel(WorkDay workDay, double contractedHours) {
            Id = workDay.Id;
            Date = workDay.Date;
            WorkDayType = workDay.WorkDayType;
            ContractId = workDay.ContractId;
            Contract = workDay.Contract;
            WorkingPeriods = workDay.WorkingPeriods;
            ContractedHours = contractedHours;

            var secondsWorked = workDay.WorkingPeriods.Sum(x => x.WorkTime.TotalSeconds);
            WorkHours = TimeSpan.FromSeconds(secondsWorked);
            double workdayTypeMultiplier = 1;
            switch (WorkDayType) {
                case WorkDayType.HalfVacation:
                    workdayTypeMultiplier = 0.5;
                    break;
                case WorkDayType.Vacation:
                case WorkDayType.SickDay:
                case WorkDayType.ChildSickDay:
                case WorkDayType.ParentalLeave:
                case WorkDayType.BankHoliday:
                    workdayTypeMultiplier = 0;
                    break;
            }
            OverhoursInSeconds = secondsWorked - (contractedHours * 60 * 60 * workdayTypeMultiplier);
            Overhours = TimeSpan.FromSeconds(OverhoursInSeconds);
        }

        public TimeSpan Overhours { get; internal set; }
        public double OverhoursInSeconds { get; internal set; }
        public bool HasOngoingWorkingDay { get; internal set; }
        public double ContractedHours { get; set; }

        public TimeSpan WorkHours { get; }
    }
}
