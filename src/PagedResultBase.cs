using System;

namespace BlazorPagination
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public int FirstRowOnPage => Math.Min((CurrentPage - 1) * PageSize + 1, RowCount);
        public int LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
    }
}