using MediatR;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Pipelines {
    public class UserIdPipeline<TIn, TOut> : IPipelineBehavior<TIn, TOut> {
        private readonly HttpContext httpContext;
        private readonly AppDbContext _context;

        public UserIdPipeline(IHttpContextAccessor accessor, AppDbContext context) {
            httpContext = accessor.HttpContext;
            _context = context;
        }

        public async Task<TOut> Handle(TIn request, CancellationToken cancellationToken, RequestHandlerDelegate<TOut> next) {
            var claim = httpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name));

            if (request is UserContext userContext) {
                userContext.UserEmail = claim.Value.ToUpper();
                userContext.User = _context.Users.Where(x => x.NormalizedEmail == userContext.UserEmail).SingleOrDefault();
            }

            return await next();
        }
    }
}
