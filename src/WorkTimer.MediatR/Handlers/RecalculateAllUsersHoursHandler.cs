using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class RecalculateAllUsersHoursHandler : IRequestHandler<RecalculateAllUsersHoursRequest, string> {

        public RecalculateAllUsersHoursHandler(AppDbContext context, ILogger<RecalculateAllUsersHoursHandler> logger) {
            Context = context;
            Logger = logger;
        }

        public AppDbContext Context { get; }
        public ILogger<RecalculateAllUsersHoursHandler> Logger { get; }

        public Task<string> Handle(RecalculateAllUsersHoursRequest request, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }


    }
}
