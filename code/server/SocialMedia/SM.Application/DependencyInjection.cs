using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SM.Application.Database;

namespace SM.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddDbContextFactory<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("EfcoreSocials")));

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        return services;
    }
}
