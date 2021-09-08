using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreastCancer.Infrastructure.Configuration
{
    public class CosmosDbConfiguration : ICosmosDbConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ClinicalInfoContainerName { get; set; }
        public string ClinicalInfoPartitionKeyPath { get; set; }
    }

    public class CosmosDbConfigurationValidation : IValidateOptions<CosmosDbConfiguration>
    {
        public ValidateOptionsResult Validate(string name, CosmosDbConfiguration options)
        {
            if (string.IsNullOrEmpty(options.ConnectionString))
            {
                return ValidateOptionsResult.Fail($"Missing required config parameter for Azure CosmosDB : {nameof(options.ConnectionString)}");
            }

            if (string.IsNullOrEmpty(options.ClinicalInfoContainerName))
            {
                return ValidateOptionsResult.Fail($"Missing required config parameter for Azure CosmosDB : { nameof(options.ClinicalInfoContainerName)}");
            }            

            if (string.IsNullOrEmpty(options.DatabaseName))
            {
                return ValidateOptionsResult.Fail($"Missing required config parameter for Azure CosmosDB : { nameof(options.DatabaseName)}");
            }

            if (string.IsNullOrEmpty(options.ClinicalInfoPartitionKeyPath))
            {
                return ValidateOptionsResult.Fail($"Missing required config parameter for Azure CosmosDB : { nameof(options.ClinicalInfoPartitionKeyPath)}");
            }           

            return ValidateOptionsResult.Success;
        }
    }
}

