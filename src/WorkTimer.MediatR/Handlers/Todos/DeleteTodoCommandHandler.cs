using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.Persistence.Data;

namespace WorkTimer.MediatR.Handlers.Todos;

public sealed class DeleteTodoCommand : UserContext, IRequest<EmptyResult>
{
    public int TodoId { get; set; }

    public DeleteTodoCommand(int todoId)
    {
        TodoId = todoId;
    }
}

public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, EmptyResult>
{
    private readonly AppDbContext _context;

    public DeleteTodoCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EmptyResult> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        _context.Todos.Remove(new Todo() { Id = command.TodoId });

        await _context.SaveChangesAsync(cancellationToken);

        return EmptyResult.Empty;
    }
}