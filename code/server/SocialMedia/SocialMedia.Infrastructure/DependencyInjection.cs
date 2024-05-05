using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Users;
using SocialMedia.Infrastructure.Repositories;
using SocialMedia.Infrastructure.Authentication;
using Microsoft.AspNetCore.Http;

namespace SocialMedia.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            AddAuthentication(services);

            AddPersistence(services, configuration);
            return services;
        }

        private static void AddAuthentication(IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
        {
            // MS SQL Server setup
            AddSQLServerConnection(services, configuration);

            // Cosmos DB setup
            AddCosmosDBConnection(services, configuration);
        }

        private static void AddSQLServerConnection(IServiceCollection services, IConfiguration configuration)
        {
            var sqlServerConnectionString = configuration.GetConnectionString("SQLServerConnection") ??
                            throw new ArgumentNullException(nameof(configuration));

            // SQL Server DbContext setup
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(sqlServerConnectionString);
            });

            // Register SQL Server repositories
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        }

        private static void AddCosmosDBConnection(IServiceCollection services, IConfiguration configuration)
        {
            var cosmosDbPrimaryKey = configuration.GetConnectionString("CosmosPrimaryKey");
            if (string.IsNullOrEmpty(cosmosDbPrimaryKey))
            {
                throw new InvalidOperationException("Cosmos DB primary key is null or empty.");
                // Example: Log.Warning("Cosmos DB primary key is null or empty. Defaulting to a fallback value.");
                // Or: cosmosDbPrimaryKey = "your_default_value_here";
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

            services.AddTransient<IUserReadRepository, UserReadRepository>();
        }

    }
}
