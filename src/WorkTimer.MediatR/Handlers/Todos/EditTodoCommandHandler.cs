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
    private readonly AppDbContext _appDbContext;

    public EditTodoCommandHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task<EmptyResult> Handle(EditTodoCommand request, CancellationToken cancellationToken)
    {


        return Task.FromResult(EmptyResult.Empty);
    }
}