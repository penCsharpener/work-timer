namespace WorkTimer.Domain.Models {
    public class PagingFilter {
        public PagingFilter(int page = 0, int pageSize = 20) {
            Page = page;
            PageSize = pageSize;
        }

        public int Page { get; }
        public int PageSize { get; }
        public int SkippedItems => Page * PageSize;
    }
}