using SM.Application.Abstractions;
using SM.Application.Shared.Models;
using SM.Domain.Shared;

namespace SM.Application.Shared.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedFeed<T>> ToPagedFeed<T>(this IQueryable<T> source, PageFilter filter)
    {
        var ordered = filter.Order?.ToLower() switch
        {
            "asc" => source.OrderBy(e => true),
            "desc" => source.OrderByDescending(e => true),
            _ => source
        };

        int pageSize = Constants.PAGE_SZIE;

        return new PagedFeed<T>
        {
            Index = filter.Index,
            Order = filter.Order,
            Values = ordered.Skip(filter.Index * pageSize).Take(pageSize).ToArray()
        };
    }
}
