using MediatR;
using System;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Requests
{
    public class OverHoursCompensationRequest : UserContext, IRequest<Nothing>
    {
        public OverHoursCompensationRequest(DateTime dateFrom, DateTime dateTo, string comment = default)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
            Comment = comment;
        }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Comment { get; set; }
    }
}
