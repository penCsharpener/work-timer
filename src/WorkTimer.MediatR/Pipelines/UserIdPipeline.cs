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

namespace WorkTimer.MediatR.Pipelines
{
    public class UserIdPipeline<TIn, TOut> : IPipelineBehavior<TIn, TOut>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserIdPipeline<TIn, TOut>> _logger;
        private readonly HttpContext httpContext;

        public UserIdPipeline(IHttpContextAccessor accessor, AppDbContext context, ILogger<UserIdPipeline<TIn, TOut>> logger)
        {
            httpContext = accessor.HttpContext;
            _context = context;
            _logger = logger;
        }

        public async Task<TOut> Handle(TIn request, CancellationToken cancellationToken, RequestHandlerDelegate<TOut> next)
        {
            if (httpContext?.User?.Claims == null)
            {
                _logger.LogError("no user or claims in httpContext");

                return await next();
            }

            Claim? claim = httpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name));

            if (string.IsNullOrEmpty(claim?.Value))
            {
                _logger.LogError("no matching claim found");

                return await next();
            }

            if (request is UserContext userContext && userContext.User == null)
            {
                userContext.UserEmail = claim.Value.ToUpper();
                userContext.User = _context.Users.Include(x => x.Contracts.Where(c => c.IsCurrent)).Where(x => x.NormalizedEmail == userContext.UserEmail).SingleOrDefault();
            }

            return await next();
        }
    }
}