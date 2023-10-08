using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string url = configuration.GetSection("AzureCosmosDBSettings").GetConnectionString("URL");  
            string primaryKey = configuration.GetSection("AzureCosmosDBSettings").GetConnectionString("PrimaryKey"); 

            services.AddSingleton<CosmosClient>(provider => new CosmosClient(url, primaryKey));

            return services;
        }
    }
}
