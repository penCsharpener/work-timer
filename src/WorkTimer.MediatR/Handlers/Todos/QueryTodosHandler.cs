using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers.Todos;

public sealed class TodosQuery : UserContext, IRequest<TodosQueryResponse>
{

}

public sealed class TodosQueryResponse
{
    public List<Todo> Todos { get; set; }
}

public class QueryTodosHandler : IRequestHandler<TodosQuery, TodosQueryResponse>
{
    private readonly AppDbContext _context;

    public QueryTodosHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TodosQueryResponse> Handle(TodosQuery request, CancellationToken cancellationToken)
    {
        var todos = await _context.Todos.Where(t => t.UserId == request.User.Id && (t.IsContractIndependent || t.ContractId == request.CurrentContract.Id)).ToListAsync(cancellationToken);

        var response = new TodosQueryResponse()
        {
            Todos = todos
        };

        return response;
    }
}