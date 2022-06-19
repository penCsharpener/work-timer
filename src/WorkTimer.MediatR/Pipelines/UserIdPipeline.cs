using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Pipelines;

public class UserIdPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserIdPipeline<TRequest, TResponse>> _logger;
    private readonly HttpContext httpContext;

    public UserIdPipeline(IHttpContextAccessor accessor, AppDbContext context, ILogger<UserIdPipeline<TRequest, TResponse>> logger)
    {
        httpContext = accessor.HttpContext;
        _context = context;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (httpContext?.User?.Claims == null)
        {
            _logger.LogError("no user or claims in httpContext");

            return await next();
        }

        var claim = httpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name));

        if (string.IsNullOrEmpty(claim?.Value))
        {
            _logger.LogError("no matching claim found");

            return await next();
        }

        if (request is UserContext userContext && (userContext.User == null || userContext.CurrentContract == null))
        {
            userContext.UserEmail = claim.Value.ToUpper();
            userContext.User = _context.Users.Include(x => x.Contracts.Where(c => c.IsCurrent)).Where(x => x.NormalizedEmail == userContext.UserEmail).SingleOrDefault();
            userContext.CurrentContract = userContext.User.Contracts.FirstOrDefault();
        }

        return await next();
    }
}