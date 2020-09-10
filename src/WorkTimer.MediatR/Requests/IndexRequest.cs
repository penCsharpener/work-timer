using MediatR;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Models;
using WorkTimer.MediatR.Responses;

namespace WorkTimer.MediatR.Requests {
    public class IndexRequest : UserContext, IRequest<IndexResponse> {
        public IndexRequest(int page = 0, int pageSize = 20) {
            PagingFilter = new PagingFilter(page, pageSize);
        }

        public PagingFilter PagingFilter { get; set; }
    }
}
