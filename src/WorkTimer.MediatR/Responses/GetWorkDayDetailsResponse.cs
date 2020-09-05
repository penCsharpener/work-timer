using MediatR;
using System.Collections.Generic;
using WorkTimer.Domain.Models;

namespace WorkTimer.MediatR.Responses {
    public class GetWorkDayDetailsResponse : IRequest<bool> {
        public WorkDay WorkDay { get; set; }
        public ICollection<WorkingPeriod> WorkingPeriods { get; set; }
        public bool IsOpenWorkday { get; set; }
    }
}
