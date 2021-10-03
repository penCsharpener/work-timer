using MediatR;
using System;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Responses {
    public class GetWorkingPeriodResponse : IRequest<bool> {
        public WorkingPeriod WorkingPeriod { get; set; }
        public UserContext UserContext { get; set; }

        public DateTime StartTime {
            get => WorkingPeriod.StartTime.ToUniversalTime();
            set => WorkingPeriod.StartTime = value;
        }

        public DateTime? EndTime {
            get {
                if (WorkingPeriod.EndTime.HasValue) {
                    return WorkingPeriod.EndTime.Value.ToUniversalTime();
                }

                return WorkingPeriod.EndTime;
            }

            set {
                if (value.HasValue) {
                    WorkingPeriod.EndTime = value.Value;
                } else {
                    WorkingPeriod.EndTime = value;
                }
            }
        }
    }
}