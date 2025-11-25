using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Shared.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedFeed<T>> ToPagedFeed<T>(this IQueryable<T> source, PageFilter filter)
    {
        filter ??= new PageFilter();

        var order = (filter.Order ?? string.Empty).Trim().ToLower();

        IQueryable<T> ordered = source;

        try
        {
            if (order == "asc")
            {
                ordered = source.OrderBy(e => true); 
            }
            else if (order == "desc")
            {
                ordered = source.OrderByDescending(e => true);
            }
        }
        catch
        {
            ordered = source;
        }

        int pageSize = Constants.PAGE_SIZE;

        return new PagedFeed<T>
        {
            Index = filter.Index,
            Order = order,
            Values = await ordered.Skip(filter.Index * pageSize).Take(pageSize).ToArrayAsync()
        };
    }
}
