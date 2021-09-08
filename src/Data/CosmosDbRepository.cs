using BreastCancer.Infrastructure.Configuration;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data
{
    public abstract class CosmosDbRepository<T> : IDataRepository<T> where T : BaseEntity
    {
        protected readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        protected readonly CosmosClient _client;

        public abstract string ContainerName { get; }

        public CosmosDbRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                           CosmosClient client)
        {
            _cosmosDbConfiguration = cosmosDbConfiguration
                    ?? throw new ArgumentNullException(nameof(cosmosDbConfiguration));

            _client = client
                    ?? throw new ArgumentNullException(nameof(client));
        }
        protected Container GetContainer()
        {
            var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
            Container container = database.GetContainer(ContainerName);
            return container;
        }

        Task<T> IDataRepository<T>.AddAsync(T newEntity)
        {
            throw new NotImplementedException();
        }

        Task<T> IDataRepository<T>.GetAsync(string entityId)
        {
            throw new NotImplementedException();
        }

        Task<T> IDataRepository<T>.UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        Task IDataRepository<T>.DeleteAsync(string entityId)
        {
            throw new NotImplementedException();
        }

        Task<IReadOnlyList<T>> IDataRepository<T>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        //public async Task<T> AddAsync(T newEntity)
        //{
        //    Container container = GetContainer();
        //    ItemResponse<T> createResponse = await container.CreateItemAsync(newEntity);
        //    //try
        //    //{
        //    //    Container container = GetContainer();
        //    //    ItemResponse<T> createResponse = await container.CreateItemAsync(newEntity);
        //    //    return createResponse.Value;
        //    //}
        //    //catch (CosmosException ex)
        //    //{
        //    //    //Log.Error($"New entity with ID: {newEntity.Id} was not added successfully - error details: {ex.Message}");

        //    //    //if (ex.Status != (int)HttpStatusCode.NotFound)
        //    //    //{
        //    //    //    throw;
        //    //    //}

        //    //    return null;
        //    //}
        //}
    }
}
