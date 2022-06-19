using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers
{
    public class DeleteContractHandler : IRequestHandler<DeleteContractRequest, bool>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteContractHandler> _logger;

        public DeleteContractHandler(AppDbContext context, ILogger<DeleteContractHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteContractRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Contract == null)
                {
                    return false;
                }

                _context.Contracts.Remove(new Domain.Models.Contract() { Id = request.Contract.Id });

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Contract with id {request.Contract.Id} of User '{request.User?.Id}' could not be deleted");

                return false;
            }
        }
    }
}