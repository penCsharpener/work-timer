using MediatR;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.MediatR.Requests {
    public class GetWorkDayDetailsRequest : UserContext, IRequest<GetWorkDayDetailsResponse> {
        public GetWorkDayDetailsRequest(int workDayId) {
            WorkDayId = workDayId;
        }

        public int WorkDayId { get; set; }
    }
}