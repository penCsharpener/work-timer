using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers.Todos;

public sealed class EditTodoCommand : UserContext, IRequest<EmptyResult>
{
    public required Todo Todo { get; set; }
}

public class EditTodoCommandHandler : IRequestHandler<EditTodoCommand, EmptyResult>
{
    private readonly AppDbContext _context;

    public EditTodoCommandHandler(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<EmptyResult> Handle(EditTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = await _context.Todos.FirstAsync(t => t.Id == request.Todo.Id, cancellationToken);

        todo.Title = request.Todo.Title;
        todo.Description = request.Todo.Description;
        todo.Deadline = request.Todo.Deadline;
        todo.Priority = request.Todo.Priority;
        todo.IsContractIndependent = request.Todo.IsContractIndependent;

        await _context.SaveChangesAsync(cancellationToken);

        return EmptyResult.Empty;
    }
}

public sealed class AddTodoCommand : UserContext, IRequest<int>
{
    public required Todo Todo { get; set; }
}

public class AddTodoCommandHandler : IRequestHandler<AddTodoCommand, int>
{
    private readonly AppDbContext _context;

    public AddTodoCommandHandler(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<int> Handle(AddTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = new Todo
        {
            Title = request.Todo.Title,
            Description = request.Todo.Description,
            Deadline = request.Todo.Deadline,
            Priority = request.Todo.Priority,
            IsContractIndependent = request.Todo.IsContractIndependent,
            UserId = request.User.Id,
            ContractId = request.CurrentContract.Id,
        };

        _context.Todos.Add(todo);

        await _context.SaveChangesAsync(cancellationToken);

        return todo.Id;
    }
}