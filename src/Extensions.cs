using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BlazorPagination
{
    public static class Extensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            page = page < 1 ? 1 : page;
            var result = new PagedResult<T> { CurrentPage = page, PageSize = pageSize, RowCount = query.Count() };
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            page = Math.Min(result.PageCount, page);
            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToArrayAsync();
            return result;
        }

        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            page = page < 1 ? 1 : page;
            var result = new PagedResult<T> { CurrentPage = page, PageSize = pageSize, RowCount = query.Count() };
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            page = Math.Min(result.PageCount, page);
            var skip = (page - 1) * pageSize;
            result.Results = query.Skip(skip).Take(pageSize).ToArray();
            return result;
        }
    }
}