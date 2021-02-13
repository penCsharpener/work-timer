using MediatR;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Requests {
    public class NewWorkingPeriodRequest : UserContext, IRequest<bool> {
        public string? Comment { get; set; }
    }
}