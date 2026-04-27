using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SM.Application.Abstractions;
using SM.Application.Behaviors;
using SM.Domain.Shared;
using System.Text.Json;

namespace SM.Infrastructure.Behaviors;

public class CachingBehavior<TRequest, TResponse>(IMemoryCache cache, ILogger<CachingBehavior<TRequest, TResponse>> logger) : ICachingBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (!IsQueryRequest())
        {
            return await next(ct);
        }

        var cacheKey = $"{typeof(TRequest).FullName}:{JsonSerializer.Serialize(request)}";

        if (cache.TryGetValue(cacheKey, out TResponse? cachedResult) 
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

    private static bool IsQueryRequest()
        => typeof(TRequest)
            .GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>));
}
