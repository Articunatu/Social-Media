using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SM.Application.Abstractions;
using SM.Application.Behaviors;
using SM.Domain.Shared;
using System.Text.Json;

namespace SM.Infrastructure.Behaviors;

public class CachingBehavior<TRequest, TResponse>(IMemoryCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger) : ICachingBehavior<TRequest, TResponse>
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
        {
            logger.LogInformation("Cache hit for key: {CacheKey}", cacheKey);
            return cachedResult;
        }

        logger.LogInformation("Cache miss for key: {CacheKey}", cacheKey);
        var result = await next(ct);

        if (result.IsSuccess)
        {
            cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
            logger.LogInformation("Cache set for key: {CacheKey}", cacheKey);
        }

        return result;
    }
}
