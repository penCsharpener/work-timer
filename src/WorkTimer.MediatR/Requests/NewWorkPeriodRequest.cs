using MediatR;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Requests {
    public class NewWorkPeriodRequest : UserContext, IRequest<bool> {
        public string? Comment { get; set; }
    }
}
