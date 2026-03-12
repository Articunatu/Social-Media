using Microsoft.Extensions.DependencyInjection;
using SM.Application.Authentication;
using SM.Infrastructure.Authentication;

namespace SM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IJwtService, JwtService>();
        return services;
    }
}
