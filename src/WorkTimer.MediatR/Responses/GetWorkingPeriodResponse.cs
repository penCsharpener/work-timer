using MediatR;
using WorkTimer.Domain.Models;

namespace WorkTimer.MediatR.Responses {
    public class GetWorkingPeriodResponse : IRequest<bool> {
        public WorkingPeriod WorkingPeriod { get; set; }
    }
}
