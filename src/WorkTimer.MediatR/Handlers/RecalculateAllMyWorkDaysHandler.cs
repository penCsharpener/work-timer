using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class RecalculateAllMyWorkDaysHandler : IRequestHandler<RecalculateAllMyWorkDaysRequest, string> {

        public RecalculateAllMyWorkDaysHandler(AppDbContext context, ILogger<RecalculateAllMyWorkDaysHandler> logger) {
            Context = context;
            Logger = logger;
        }

        public AppDbContext Context { get; }
        public ILogger<RecalculateAllMyWorkDaysHandler> Logger { get; }

        public Task<string> Handle(RecalculateAllMyWorkDaysRequest request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

    }
}
