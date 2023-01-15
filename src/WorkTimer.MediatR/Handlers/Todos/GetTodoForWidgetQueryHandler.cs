using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.Domain.Models.Enums;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers.Todos;

public sealed class GetTodoForWidgetQuery : UserContext, IRequest<GetTodoForWidgetQueryResult> { }

public sealed class GetTodoForWidgetQueryResult
{
    public List<Todo> Todos { get; set; }
}

public sealed class GetTodoForWidgetQueryHandler : IRequestHandler<GetTodoForWidgetQuery, GetTodoForWidgetQueryResult>
{
    private readonly AppDbContext _context;

    public GetTodoForWidgetQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<GetTodoForWidgetQueryResult> Handle(GetTodoForWidgetQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.Now;

        var todos = await _context.Todos.Where(t => ((t.Deadline.HasValue && t.Deadline.Value.Date == now.Date) || t.Priority >= TodoPriority.High) && !t.IsCompleted && t.UserId == request.User.Id && (t.IsContractIndependent || t.ContractId == request.CurrentContract.Id))
            .OrderByDescending(t => t.Deadline)
            .ThenByDescending(t => t.Priority).Take(5).ToListAsync(cancellationToken);

        return new() { Todos = todos };
    }
}