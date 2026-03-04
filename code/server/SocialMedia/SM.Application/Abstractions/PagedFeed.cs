using SM.Application.Shared.Models;

namespace SM.Application.Abstractions;

public record PagedFeed<T> : PageFilter
{
    public IEnumerable<T> Values { get; init; } = [];
}
