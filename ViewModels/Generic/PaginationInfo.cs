namespace Aban.ViewModel
{
    public class PaginationInfo : X.PagedList.IPagedList
    {
        public PaginationInfo()
        {

        }

        public PaginationInfo(X.PagedList.IPagedList model)
        {
            PageSize = model.PageSize;
            HasPreviousPage = model.HasPreviousPage;
            HasNextPage = model.HasNextPage;
            IsFirstPage = model.IsFirstPage;
            IsLastPage = model.IsLastPage;
            FirstItemOnPage = model.FirstItemOnPage;
            LastItemOnPage = model.LastItemOnPage;
            PageCount = model.PageCount;
            TotalItemCount = model.TotalItemCount;
            PageNumber = model.PageNumber;
        }

        public int PageSize { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public int FirstItemOnPage { get; set; }

        public int LastItemOnPage { get; set; }

        public int PageCount { get; set; }

        public int TotalItemCount { get; set; }

        public int PageNumber { get; set; }
    }

}