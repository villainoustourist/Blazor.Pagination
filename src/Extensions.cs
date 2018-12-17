using System;
using System.Linq;

namespace BlazorPagination
{
    public static class Extensions
    {
        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            page = page < 1 ? 1 : page;
            var result = new PagedResult<T> { CurrentPage = page, PageSize = pageSize, RowCount = query.Count() };
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToList();
            return result;
        }
    }
}