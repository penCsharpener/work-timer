using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Requests;

namespace WorkTimer.MediatR.Handlers {
    public class RestrictRegistrationHandler : IRequestHandler<RestrictRegistrationRequest, bool> {
        private readonly RestrictRegistration _config;
        private readonly ILogger<RestrictRegistrationHandler> _logger;

        public RestrictRegistrationHandler(IConfiguration config, ILogger<RestrictRegistrationHandler> logger) {
            _config = config.GetSection("ApplicationSettings:RestrictRegistration").Get<RestrictRegistration>();
            _logger = logger;
        }

        public Task<bool> Handle(RestrictRegistrationRequest request, CancellationToken cancellationToken) {
            try {
                var userDomain = request.UserEmail.Split('@', StringSplitOptions.RemoveEmptyEntries)[1];

                if (_config.TotalLockDown) {
                    _logger.LogInformation($"{request.UserEmail} was blocked (total lock down).");
                    return Task.FromResult(false);
                }

                // if no mails are explicitely permitted, we permit all emails except those that are explicitely blocked
                if (_config.PermittedDomains.Length == 0 && _config.PermittedEmails.Length == 0) {
                    return Task.FromResult(CheckBlocked(request.UserEmail, userDomain));
                } else {
                    if (_config.PermittedEmails.Any(x => x.Equals(request.UserEmail, StringComparison.InvariantCultureIgnoreCase))) {
                        return Task.FromResult(true);
                    }

                    if (_config.PermittedDomains.Any(x => x.Equals(userDomain, StringComparison.InvariantCultureIgnoreCase))) {
                        return Task.FromResult(true);
                    }
                }

                return Task.FromResult(CheckBlocked(request.UserEmail, userDomain));

            } catch (Exception ex) {
                _logger.LogError(ex, $"{request.UserEmail}: Registration restrictions could not be checked.");
                return Task.FromResult(false);
            }
        }

        public bool CheckBlocked(string userEmail, string userDomain) {
            if (_config.BlockedEmails.Any(x => userEmail.Equals(x, StringComparison.InvariantCultureIgnoreCase))) {
                _logger.LogInformation($"{userEmail} was blocked (blocked email).");
                return false;
            }

            if (_config.BlockedDomains.Any(x => userDomain.Equals(x, StringComparison.InvariantCultureIgnoreCase))) {
                _logger.LogInformation($"{userEmail} was blocked (blocked domain).");
                return false;
            }

            return true;
        }
    }
}
