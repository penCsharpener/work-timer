using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;

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
    public Task<TodosQueryResponse> Handle(TodosQuery request, CancellationToken cancellationToken)
    {

        var todos = Enumerable.Range(1, 200).Select(i => new Todo()
        {
            Id = i,
            ContactId = i % 23 == 0 ? 2 : 1,
            CreatedOn = DateTime.Now.AddDays(-i + 1),
            Deadline = i % 17 == 0 ? DateTime.Now.AddDays(i) : null,
            Description = $"Description " + i,
            IsContractIndependent = i % 47 == 0,
            Priority = i % 19 == 0 ? Domain.Models.Enums.TodoPriority.High : Domain.Models.Enums.TodoPriority.Medium,
            Title = "Title " + i,
            UserId = 1,
        }).OrderBy(t => t.Deadline ?? DateTime.MaxValue)
        .ThenByDescending(t => t.Priority)
        .ThenBy(t => t.CreatedOn)
        .ToList();

        var response = new TodosQueryResponse()
        {
            Todos = todos
        };

        return Task.FromResult(response);
    }
}