﻿using BreastCancer.Infrastructure.Configuration;
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
        public ClinicalInfoRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                 CosmosClient client, ILogger<CosmosDbRepository<ClinicalInfo>> logger) : base(cosmosDbConfiguration, client, logger)
        {
        }

        public override string ContainerName => _cosmosDbConfiguration.ClinicalInfoContainerName;

        public async Task<ClinicalInfo> GetExistingReservationByCarIdAsync(int clinicalInfoId)
        {
            //try
            //{
            //    CosmosContainer container = GetContainer();
            //    var entities = new List<CarReservation>();
            //    QueryDefinition queryDefinition = new QueryDefinition("select * from c where c.rentTo > @rentFrom AND c.carId = @carId")
            //        .WithParameter("@rentFrom", rentFrom)
            //        .WithParameter("@carId", carId);

            //    AsyncPageable<CarReservation> queryResultSetIterator = container.GetItemQueryIterator<CarReservation>(queryDefinition);

            //    await foreach (CarReservation carReservation in queryResultSetIterator)
            //    {
            //        entities.Add(carReservation);
            //    }

            //    return entities.FirstOrDefault();
            //}
            //catch (CosmosException ex)
            //{
            //    Log.Error($"Entity with ID: {carId} was not retrieved successfully - error details: {ex.Message}");

            //    if (ex.ErrorCode != "404")
            //    {
            //        throw;
            //    }

            //    return null;
            //}

            return new ClinicalInfo();
        }
                
    }
}