using Microsoft.EntityFrameworkCore;

namespace SM.Application.Abstractions;

public class PagedFeed<T> : PageFilter
{
    public IEnumerable<T>? Values { get; set; }
}

public class PageFilter
{
    public int Index { get; set; }
    public string? Order { get; set; }
}

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

        return new PagedFeed<T>
        {
            Index = filter.Index,
            Order = filter.Order,
            Values = ordered.Skip(filter.Index * 20).Take(20).ToArrayAsync()
        };
    }
}
