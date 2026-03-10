using MediatR;
using Microsoft.Extensions.Caching.Memory;
using SM.Application.Abstractions;
using SM.Application.Behaviors;
using SM.Domain.Shared;
using System.Text.Json;

namespace SM.Infrastructure.Behaviors;

public class CachingBehavior<TRequest, TResponse>(IMemoryCache cache) : ICachingBehavior<TRequest, TResponse>
    where TRequest : IQuery<TResponse>
{
    public async Task<Result<TResponse>> Handle(
        TRequest request,
        RequestHandlerDelegate<Result<TResponse>> next,
        CancellationToken ct)
    {
        var cacheKey = $"{typeof(TRequest).FullName}:{JsonSerializer.Serialize(request)}";

        if (cache.TryGetValue(cacheKey, out Result<TResponse>? cachedResult) 
                && cachedResult != null)
            return cachedResult;

        var result = await next(ct);

        if (result.IsSuccess)
            cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

        return result;
    }
}
