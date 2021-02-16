using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers
{
    public class GetWorkMonthsRequest : UserContext, IRequest<GetWorkMonthsResponse>
    {

    }

    public class GetWorkMonthsResponse
    {
        public List<WorkMonthsListModel> Months { get; set; }
    }

    public class GetWorkMonthsHandler : IRequestHandler<GetWorkMonthsRequest, GetWorkMonthsResponse>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetWorkMonthsHandler> _logger;

        public GetWorkMonthsHandler(AppDbContext context, ILogger<GetWorkMonthsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<GetWorkMonthsResponse> Handle(GetWorkMonthsRequest request, CancellationToken cancellationToken)
        {
            var response = new GetWorkMonthsResponse();

            try
            {
                var contract = request.User.Contracts.FirstOrDefault(x => x.IsCurrent);
                response.Months = (await _context.WorkMonths.Include(x => x.WorkDays).Where(x => x.UserId == request.User.Id)
                    .Select(x => new { x.Id, x.DaysWorked, x.TotalOverhours, x.TotalHours, x.Year, x.Month, x.WorkDays })
                    .ToListAsync())
                    .Select(x => new WorkMonthsListModel(x.Id, x.DaysWorked, Math.Round(x.TotalOverhours, 1), Math.Round(x.TotalHours, 1), x.WorkDays.Sum(x => x.Contract.GetContractedHoursPerDay() * x.WorkDayType.GetWorkHourMultiplier()), x.Year, x.Month))
                    .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                    .ToList();


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return response;
        }
    }

    public record WorkMonthsListModel(int Id, int DaysWorked, double TotalOverhours, double TotalHours, double RequiredHours, int Year, int Month);
}
