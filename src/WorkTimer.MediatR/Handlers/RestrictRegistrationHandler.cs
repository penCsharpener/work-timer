using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Handlers;

public class RestrictRegistrationRequest : IRequest<bool>
{
    public RestrictRegistrationRequest(string userEmail)
    {
        UserEmail = userEmail;
    }

    public string UserEmail { get; set; }
}

public class RestrictRegistrationHandler : IRequestHandler<RestrictRegistrationRequest, bool>
{
    private readonly RestrictRegistration _config;
    private readonly ILogger<RestrictRegistrationHandler> _logger;

    public RestrictRegistrationHandler(IConfiguration config, ILogger<RestrictRegistrationHandler> logger)
    {
        _config = config.GetSection("ApplicationSettings:RestrictRegistration").Get<RestrictRegistration>();
        _logger = logger;
    }

    public Task<bool> Handle(RestrictRegistrationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userDomain = request.UserEmail.Split('@', StringSplitOptions.RemoveEmptyEntries)[1];

            if (_config.TotalLockDown)
            {
                _logger.LogInformation("{UserEmail} was blocked (total lock down).", request.UserEmail);

                return Task.FromResult(false);
            }

            // if no mails are explicitely permitted, we permit all emails except those that are explicitely blocked
            if (_config.PermittedDomains?.Length == 0 && _config.PermittedEmails?.Length == 0)
            {
                return Task.FromResult(CheckBlocked(request.UserEmail, userDomain));
            }

            if (_config.PermittedEmails?.Any(x => x.Equals(request.UserEmail, StringComparison.InvariantCultureIgnoreCase)) == true)
            {
                return Task.FromResult(true);
            }

            if (_config.PermittedDomains?.Any(x => x.Equals(userDomain, StringComparison.InvariantCultureIgnoreCase)) == true)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(CheckBlocked(request.UserEmail, userDomain));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{UserEmail}: Registration restrictions could not be checked.", request.UserEmail);

            return Task.FromResult(false);
        }
    }

    public bool CheckBlocked(string userEmail, string userDomain)
    {
        if (_config.BlockedEmails?.Any(x => userEmail.Equals(x, StringComparison.InvariantCultureIgnoreCase)) == true)
        {
            _logger.LogInformation("{userEmail} was blocked (blocked email).", userEmail);

            return false;
        }

        if (_config.BlockedDomains?.Any(x => userDomain.Equals(x, StringComparison.InvariantCultureIgnoreCase)) == true)
        {
            _logger.LogInformation("{userEmail} was blocked (blocked domain).", userEmail);

            return false;
        }

        return true;
    }
}