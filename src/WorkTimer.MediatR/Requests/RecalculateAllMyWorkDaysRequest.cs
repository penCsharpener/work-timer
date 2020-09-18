using MediatR;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Requests {
    public class RecalculateAllMyWorkDaysRequest : UserContext, IRequest<string> {

    }
}
