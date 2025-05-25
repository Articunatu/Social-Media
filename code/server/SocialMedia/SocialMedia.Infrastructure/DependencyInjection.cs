using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
//using Microsoft.AspNetCore.Http;
using SocialMedia.Domain.Users.Authentication;
using SocialMedia.Application.Profiles.GetProfilePosts;
using SocialMedia.Infrastructure.Profiles;

namespace SocialMedia.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddAuthentication(services);

        AddPersistence(services, configuration);
        return services;
    }

    private static void AddAuthentication(IServiceCollection services)
    {
        //services.AddScoped<IAuthenticationService, AuthenticationService>();
        //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        AddCosmosDBConnection(services, configuration);
    }

    private static void AddCosmosDBConnection(IServiceCollection services, IConfiguration configuration)
    {
        var cosmosDbPrimaryKey = configuration.GetConnectionString("CosmosPrimaryKey");
        if (string.IsNullOrEmpty(cosmosDbPrimaryKey))
        {
            throw new InvalidOperationException("Cosmos DB primary key is null or empty.");
        }

        services.AddTransient<CosmosClient>(sp =>
        {
            return new CosmosClient(configuration.GetConnectionString("CosmosEndpointUri"), cosmosDbPrimaryKey, new CosmosClientOptions
            {
                SerializerOptions = new CosmosSerializationOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });

        });

        services.AddTransient<Container>(sp =>
        {
            var cosmosClient = sp.GetRequiredService<CosmosClient>();
            var cosmosContainer = cosmosClient.GetContainer(configuration.GetConnectionString("CosmosPrimaryKey"), "Account");
            return cosmosContainer;
        });

        services.AddTransient<IProfileRepository, ProfileRepository>();
    }

}
