using System;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.MediatR.Models;

namespace WorkTimer.MediatR.Handlers.Todos;

public sealed class DeleteTodoCommand : UserContext, IRequest<EmptyResult>
{

}

public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, EmptyResult>
{
    public Task<EmptyResult> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}