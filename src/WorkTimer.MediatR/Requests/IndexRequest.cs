using MediatR;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.MediatR.Requests {
    public class IndexRequest : UserContext, IRequest<IndexResponse> {

    }
}
