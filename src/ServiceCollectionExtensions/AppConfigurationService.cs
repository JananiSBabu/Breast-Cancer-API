using BreastCancer.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI
{
    public static class AppConfigurationService
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CosmosDbConfiguration>(config.GetSection("CosmosDbSettings"));
            services.AddSingleton<IValidateOptions<CosmosDbConfiguration>, CosmosDbConfigurationValidation>();
            var cosmosDbConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<CosmosDbConfiguration>>().Value;
            services.AddSingleton<ICosmosDbConfiguration>(cosmosDbConfiguration);

            return services;
        }
    }
}
