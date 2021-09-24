using BreastCancer.Infrastructure.Configuration;
using BreastCancerAPI.Data.Entities;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Linq;
using Azure;

namespace BreastCancerAPI.Data
{
    public abstract class CosmosDbRepository<T> : IDataRepository<T> where T : BaseEntity
    {
        protected readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        protected readonly CosmosClient _client;
        private readonly ILogger<CosmosDbRepository<T>> _logger;

        public abstract string ContainerName { get; }

        public CosmosDbRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                           CosmosClient client, ILogger<CosmosDbRepository<T>> logger)
        {
            _cosmosDbConfiguration = cosmosDbConfiguration
                    ?? throw new ArgumentNullException(nameof(cosmosDbConfiguration));

            _client = client
                    ?? throw new ArgumentNullException(nameof(client));

            _logger = logger;
        }
        protected Container GetContainer()
        {
            var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
            Container container = database.GetContainer(ContainerName);
            return container;
        }

        public async Task<T> AddAsync(T newEntity)
        {
            try
            {
                Container container = GetContainer();
                ItemResponse<T> createResponse = await container.CreateItemAsync(newEntity);
                return createResponse.Resource;
            }
            catch (CosmosException ex)
            {
                _logger.LogError($"New entity {newEntity.Id} was not added successfully. {ex.Message}");
                if (ex.StatusCode != HttpStatusCode.NotFound)
                {
                    throw;
                }
                return null;
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            try
            {
                Container container = GetContainer();
                //AsyncPageable<T> queryResultSetIterator = container.GetItemQueryIterator<T>();
                List<T> entities = new List<T>();
                //await foreach (var entity in queryResultSetIterator)
                //{
                //    entities.Add(entity);
                //}
                //return entities;

                using (var iterator = container.GetItemQueryIterator<T>(
                                        queryText: $"SELECT * FROM c "))
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

        public async Task<T> GetAsync(string entityId)
        {
            try
            {
                Container container = GetContainer();
                // If partition key is known from request use ReadItemAsync()
                // ItemResponse<T> entityResult = await container.ReadItemAsync<T>(entityId, new PartitionKey(entityId));

                //If Partition key is not known from request, query using Id 
                IQueryable<T> queryable = container.GetItemLinqQueryable<T>(true);
                var iterator = queryable.Where<T>(item => item.Id == entityId).ToFeedIterator();
                //return await iterator.ReadNextAsync().Result;
                //Asynchronous query execution
                while (iterator.HasMoreResults)
                {
                    foreach (var item in await iterator.ReadNextAsync())
                    {
                        Console.WriteLine(item.Id);
                        return item;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Entity {entityId} cannot be retrieved successfully. {ex.Message}");
                return null;
            }
        }

        Task<T> IDataRepository<T>.UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        Task IDataRepository<T>.DeleteAsync(string entityId)
        {
            throw new NotImplementedException();
        }

        

    }
}
