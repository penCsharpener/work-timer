using System;
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
    private static readonly Random random = new(4);

    public GetTodoQueryHandler(AppDbContext context)
    {
        _context = context;
    }

    public Task<GetTodoQueryResult> Handle(GetTodoQuery request, CancellationToken cancellationToken)
    {
        var result = new GetTodoQueryResult
        {
            Todo = new Todo()
            {
                Id = request.Id,
                ContactId = 1,
                CreatedOn = DateTime.Now,
                Deadline = DateTime.Now.AddDays(random.NextInt64(1, 3)),
                IsContractIndependent = true,
                Priority = (Domain.Models.Enums.TodoPriority) random.NextInt64(0, 3),
                Title = "Title " + request.Id,
                UserId = request.User.Id
            }
        };

        return Task.FromResult(result);
    }
}