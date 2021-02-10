using MediatR;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Requests {
    public class DeleteWorkDayRequest : UserContext, IRequest<bool> {
        public DeleteWorkDayRequest(WorkDay workDay) {
            WorkDay = workDay;
        }

        public WorkDay WorkDay { get; set; }
    }
}