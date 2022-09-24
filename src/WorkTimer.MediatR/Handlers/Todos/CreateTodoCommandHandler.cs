using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Handlers.Todos;

public sealed class CreateTodoCommand : UserContext, IRequest<EmptyResult>
{

}

public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, EmptyResult>
{
    public Task<EmptyResult> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}