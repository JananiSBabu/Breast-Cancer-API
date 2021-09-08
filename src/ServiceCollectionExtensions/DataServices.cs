using BreastCancer.Infrastructure.Configuration;
using BreastCancerAPI.Data;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BreastCancerAPI.ServiceCollectionExtensions
{
    public static class DataServices
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {

            var provider = services.BuildServiceProvider();
            var cosmoDbConfiguration = provider.GetRequiredService<ICosmosDbConfiguration>();
            CosmosClient cosmosClient = new CosmosClient(cosmoDbConfiguration.ConnectionString);

            // Create database and/or containiner if doesnot exist
            var databaseResponse = cosmosClient.CreateDatabaseIfNotExistsAsync(cosmoDbConfiguration.DatabaseName)
                                                       .GetAwaiter()
                                                       .GetResult();
            databaseResponse.Database.CreateContainerIfNotExistsAsync(
                    cosmoDbConfiguration.ClinicalInfoContainerName,
                    cosmoDbConfiguration.ClinicalInfoPartitionKeyPath,
                    400)
                    .GetAwaiter()
                    .GetResult();
            services.AddSingleton<CosmosClient>(cosmosClient);

            //var docClient = new CosmosClient(Configuration.GetConnectionString("CosmosDB"));
            //services.AddSingleton<CosmosClient>(docClient);

            services.AddSingleton<IClinicalInfoRepository, ClinicalInfoRepository>();

            return services;
        }
    }
}
