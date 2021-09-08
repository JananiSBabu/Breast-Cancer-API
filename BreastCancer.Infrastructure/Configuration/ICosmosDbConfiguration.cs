namespace BreastCancer.Infrastructure.Configuration
{
    public interface ICosmosDbConfiguration
    {
        string ClinicalInfoContainerName { get; set; }
        string ClinicalInfoPartitionKeyPath { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}