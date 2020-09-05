using MediatR;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.MediatR.Requests {
    public class GetWorkingPeriodRequest : IRequest<GetWorkingPeriodResponse> {
        public GetWorkingPeriodRequest(int workDayId, int workingPeriodId) {
            WorkDayId = workDayId;
            WorkingPeriodId = workingPeriodId;
        }

        public int WorkDayId { get; set; }
        public int WorkingPeriodId { get; set; }
    }
}
