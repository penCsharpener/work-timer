using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Extensions;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers
{
    public class OverHoursCompensationHandler : IRequestHandler<OverHoursCompensationRequest, Nothing>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OverHoursCompensationHandler> _logger;

        public OverHoursCompensationHandler(AppDbContext context, ILogger<OverHoursCompensationHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Nothing> Handle(OverHoursCompensationRequest request, CancellationToken cancellationToken)
        {
            if (request.CurrentContract == null)
            {
                throw new ArgumentNullException(nameof(request.CurrentContract));
            }

            var overHoursInTimeInterval = await _context.WorkDays.Where(w => w.ContractId == request.CurrentContract.Id && w.Date >= request.DateFrom && w.Date <= request.DateTo).SumAsync(w => w.GetOverhours());

            var overHoursCompensation = new OverHoursCompensation()
            {
                ContractId = request.CurrentContract.Id,
                DateFrom = request.DateFrom,
                DateTo = request.DateTo,
                Comment = request.Comment,
                OverHours = overHoursInTimeInterval
            };

            _context.OverHoursCompensations.Add(overHoursCompensation);

            await _context.SaveChangesAsync();

            return Nothing.Value;
        }
    }
}
