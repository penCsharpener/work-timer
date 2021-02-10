using MediatR;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.MediatR.Requests {
    public class AdminRequest : IRequest<AdminResponse> {
        public bool CalculateZeroHourWorkDays { get; set; }
        public bool RecalculateAllMyWorkDays { get; set; }
        public bool RecalculateAllUsersHours { get; set; }
    }
}