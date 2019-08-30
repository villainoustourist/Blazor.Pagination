using System.Collections.Generic;

namespace BlazorPagination
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public T[] Results { get; set; }
    }
}