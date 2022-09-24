using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Handlers.Todos;

public sealed class EditTodoCommand : UserContext, IRequest<EmptyResult>
{

}

public class EditTodoCommandHandler : IRequestHandler<EditTodoCommand, EmptyResult>
{
    public Task<EmptyResult> Handle(EditTodoCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}