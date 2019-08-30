using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPagination
{
    public static class Extensions
    {
        public async static Task<PagedResult<T>> ToPagedResult<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            page = page < 1 ? 1 : page;
            var result = new PagedResult<T> { CurrentPage = page, PageSize = pageSize, RowCount = query.Count() };
            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);
            var skip = (page - 1) * pageSize;
            result.Results = await query.Skip(skip).Take(pageSize).ToArrayAsync();
            return result;
        }
    }
}