using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class CalculateZeroHourWorkDaysHandler : IRequestHandler<CalculateZeroHourWorkDaysRequest, string> {

        public CalculateZeroHourWorkDaysHandler(AppDbContext context, ILogger<CalculateZeroHourWorkDaysHandler> logger) {
            Context = context;
            Logger = logger;
        }

        public AppDbContext Context { get; }
        public ILogger<CalculateZeroHourWorkDaysHandler> Logger { get; }

        public Task<string> Handle(CalculateZeroHourWorkDaysRequest request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }
}
