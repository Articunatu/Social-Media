using Microsoft.Extensions.DependencyInjection;
using SM.Application.Authentication;
using SM.Application.Behaviors;
using SM.Infrastructure.Authentication;
using SM.Infrastructure.Behaviors;

namespace SM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IJwtService, JwtService>();
        return services;
    }
}
