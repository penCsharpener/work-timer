using MediatR;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.MediatR.Requests
{
    public class NewWorkingPeriodRequest : UserContext, IRequest<EmptyResult>
    {
        public string? Comment { get; set; }
    }
}