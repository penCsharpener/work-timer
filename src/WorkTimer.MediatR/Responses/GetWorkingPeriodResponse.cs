using MediatR;
using System;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Responses
{
    public class GetWorkingPeriodResponse : IRequest<bool>
    {
        public WorkingPeriod WorkingPeriod { get; set; }
        public UserContext UserContext { get; set; }

        public DateTime? StartDate { get; set; } = DateTime.Now;
        public TimeSpan? StartTime { get; set; } = DateTime.Now.TimeOfDay;

        public DateTime? EndDate { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}