using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Domain.Abstractions;
using SocialMedia.Domain.Users;
using SocialMedia.Infrastructure.Repositories;
using Microsoft.Azure.Cosmos;
using SocialMedia.Domain.Users.Events;

namespace SocialMedia.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            AddPersistence(services, configuration);
            return services;
        }

        private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
        {
            AddSQLServerConnection(services, configuration);

            AddCosmosDBConnection(services, configuration);
        }

        private static void AddSQLServerConnection(IServiceCollection services, IConfiguration configuration)
        {
            var sqlServerConnectionString = configuration["SQLServerConnection:ConnectionString"] ??
                            throw new ArgumentNullException(nameof(configuration));


            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(sqlServerConnectionString);
            });

            services.AddScoped<IUserWriteRepository, UserWriteRepository>();

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        }

        private static void AddCosmosDBConnection(IServiceCollection services, IConfiguration configuration)
        {
            var cosmosDbPrimaryKey = configuration["CosmosDBConnection:PrimaryKey"];
            if (string.IsNullOrEmpty(cosmosDbPrimaryKey))
            {
                throw new InvalidOperationException("Cosmos DB primary key is null or empty.");
            }

            services.AddTransient<CosmosClient>(sp =>
            {
                return new CosmosClient(configuration["CosmosDBConnection:EndpointUri"], cosmosDbPrimaryKey, new CosmosClientOptions
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
                var cosmosContainer = cosmosClient.GetContainer(configuration["CosmosDBConnection:Database"], "Account");
                return cosmosContainer;
            });

            services.AddTransient<IUserReadRepository, UserReadRepository>();
        }

    }
}
