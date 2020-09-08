using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace BlazorPagination
{
    public static class Extensions
    {

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string column, string direction)
        {
            try
            {
                if (source != null)
                {
                    var expression = source.Expression;
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var selector = Expression.PropertyOrField(parameter, column);
                    var method = direction.Equals("desc")
                        ? "OrderByDescending"
                        : "OrderBy";
                    expression = Expression.Call(typeof(Queryable), method,
                        new[] { source.ElementType, selector.Type },
                        expression, Expression.Quote(Expression.Lambda(selector, parameter)));
                    return source.Provider.CreateQuery<T>(expression);
                }
            }
            catch (Exception)
            {
                // ignored if invalid sort (i.e. property doesn't exist)
            }

            return source;
        }

        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var rows = query.Count();
            page = page < 1 ? 1 : page;
            var result = new PagedResult<T> { CurrentPage = page, PageSize = pageSize, RowCount = rows };
            if (rows > 0)
            {
                var pageCount = (double)result.RowCount / pageSize;
                result.PageCount = (int)Math.Ceiling(pageCount);
                page = Math.Min(result.PageCount, page);
                var skip = (page - 1) * pageSize;
                result.Results = await query.Skip(skip).Take(pageSize).ToArrayAsync();
            }
            else
            {
                result.Results = await query.ToArrayAsync();
            }
            return result;
        }

        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var rows = query.Count();
            page = page < 1 ? 1 : page;
            var result = new PagedResult<T> { CurrentPage = page, PageSize = pageSize, RowCount = query.Count() };
            if (rows > 0)
            {
                var pageCount = (double)result.RowCount / pageSize;
                result.PageCount = (int)Math.Ceiling(pageCount);
                page = Math.Min(result.PageCount, page);
                var skip = (page - 1) * pageSize;
                result.Results = query.Skip(skip).Take(pageSize).ToArray();
            }
            else
            {
                result.Results = query.ToArray();
            }
            return result;
        }
    }
}