using MediatR;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Requests {
    public class DeleteWorkingPeriodRequest : UserContext, IRequest<bool> {
        public DeleteWorkingPeriodRequest(WorkingPeriod workingPeriod) {
            WorkingPeriod = workingPeriod;
        }

        public WorkingPeriod WorkingPeriod { get; set; }
    }
}
