using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Responses;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers {
    public class EditContractHandler : IRequestHandler<GetContractResponse, bool> {
        private readonly AppDbContext _context;
        private readonly ILogger<EditContractHandler> _logger;

        public EditContractHandler(AppDbContext context, ILogger<EditContractHandler> logger) {
            _context = context;
            _logger = logger;
        }

        public Task<bool> Handle(GetContractResponse request, CancellationToken cancellationToken) {
            try {
                Contract? contractToEdit = _context.Contracts.Where(x => x.UserId == request.User.Id && x.Id == request.Id).FirstOrDefault();

                if (request.IsCurrent && contractToEdit.IsCurrent != request.IsCurrent) {
                    List<Contract> currentContracts = _context.Contracts.Where(x => x.UserId == request.User.Id && x.IsCurrent).ToList();

                    foreach (Contract contract in currentContracts) {
                        contract.IsCurrent = false;
                    }

                    _context.SaveChanges();
                }

                contractToEdit.Name = request.Name;
                contractToEdit.Employer = request.Employer;
                contractToEdit.HoursPerWeek = request.HoursPerWeek;
                contractToEdit.IsCurrent = request.IsCurrent;

                _context.SaveChanges();

                return Task.FromResult(true);
            } catch (Exception ex) {
                _logger.LogError(ex, "Could not edit contract");

                return Task.FromResult(false);
            }
        }
    }
}