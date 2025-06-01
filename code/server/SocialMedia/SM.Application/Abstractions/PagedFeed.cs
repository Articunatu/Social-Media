using SM.Application.Shared.Models;

namespace SM.Application.Abstractions;

public class PagedFeed<T> : PageFilter
{
    public IEnumerable<T>? Values { get; set; }
}
