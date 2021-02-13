using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers.Stats
{
    public class RecalculateMyMonthsRequest : IRequest<RecalculateMyMonthsResponse>
    {
        private readonly int _year;
        private readonly int _month;

        public RecalculateMyMonthsRequest(int year, int month)
        {
            _year = year;
            _month = month;
        }
    }

    public class RecalculateMyMonthsResponse
    {

    }

    public class RecalculateMyMonthsHandler : IRequestHandler<RecalculateMyMonthsRequest, RecalculateMyMonthsResponse>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<RecalculateMyMonthsHandler> _logger;

        public RecalculateMyMonthsHandler(AppDbContext context, ILogger<RecalculateMyMonthsHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<RecalculateMyMonthsResponse> Handle(RecalculateMyMonthsRequest request, CancellationToken cancellationToken)
        {
            var response = new RecalculateMyMonthsResponse();


            return response;
        }
    }
}
