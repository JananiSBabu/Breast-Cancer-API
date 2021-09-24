using BreastCancer.Infrastructure.Configuration;
using BreastCancerAPI.Data.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data
{
    public class ClinicalInfoRepository : CosmosDbRepository<ClinicalInfo>, IClinicalInfoRepository
    {
        private readonly ILogger<CosmosDbRepository<ClinicalInfo>> _logger;

        public ClinicalInfoRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                 CosmosClient client, ILogger<CosmosDbRepository<ClinicalInfo>> logger) : base(cosmosDbConfiguration, client, logger)
        {
            _logger = logger;
        }

        public override string ContainerName => _cosmosDbConfiguration.ClinicalInfoContainerName;

        public async Task<IReadOnlyList<ClinicalInfo>> GetExistingReservationByCarIdAsync(string breastCancerType)
        {
            try
            {
                Container container = GetContainer();
                List<ClinicalInfo> entities = new List<ClinicalInfo>();
                QueryDefinition query = new QueryDefinition("SELECT * FROM ClinicalInfo f WHERE f.BreastCancerType = @breastCancerType")
                .WithParameter("@breastCancerType", breastCancerType);

                using (var iterator = container.GetItemQueryIterator<ClinicalInfo>(
                                        query, requestOptions: new QueryRequestOptions()
                                        {
                                            PartitionKey = new PartitionKey("BreastCancerType")
                                        }))
                {
                    while (iterator.HasMoreResults)
                    {
                        var response = await iterator.ReadNextAsync();

                        entities.AddRange(response.ToList());
                    }
                }
                return entities;
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"Entities were not retrieved successfully - error details: {ex.Message}");
                return null;
            }
        }
                
    }
}
