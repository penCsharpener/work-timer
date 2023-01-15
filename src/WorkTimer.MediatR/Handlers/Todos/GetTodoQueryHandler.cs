using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers.Todos;

public sealed class GetTodoQuery : UserContext, IRequest<GetTodoQueryResult>
{
    public required int Id { get; set; }
}

public sealed class GetTodoQueryResult
{
    public Todo Todo { get; set; }
}

public sealed class GetTodoQueryHandler : IRequestHandler<GetTodoQuery, GetTodoQueryResult>
{
    private readonly AppDbContext _context;

    public GetTodoQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetTodoQueryResult> Handle(GetTodoQuery request, CancellationToken cancellationToken)
    {
        var todo = await _context.Todos.Where(t => t.Id == request.Id && t.UserId == request.User.Id && (t.IsContractIndependent || t.ContractId == request.CurrentContract.Id)).FirstAsync(cancellationToken);

        return new() { Todo = todo };
    }
}