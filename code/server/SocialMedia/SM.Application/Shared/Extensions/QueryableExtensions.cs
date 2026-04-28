using Microsoft.EntityFrameworkCore;
using SM.Application.Abstractions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Shared.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedFeed<T>> ToPagedFeed<T>(this IQueryable<T> source, PageFilter? filter)
    {
        filter ??= new PageFilter();
        if (filter.Index < 0) 
            filter = filter with { Index = 0 };

        int pageSize = Constants.PAGE_SIZE;
        var values = await source.Skip(filter.Index * pageSize).Take(pageSize).ToArrayAsync();

        return new PagedFeed<T>
        {
            Index = filter.Index,
            Order = filter.Order,
            Values = values
        };
    }
}
