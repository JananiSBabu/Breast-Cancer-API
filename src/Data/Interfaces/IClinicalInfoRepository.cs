using BreastCancerAPI.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data
{
    public interface IClinicalInfoRepository : IDataRepository<ClinicalInfo>
    {
        public Task<IReadOnlyList<ClinicalInfo>> GetExistingReservationByCarIdAsync(string breastCancerType);
    }
}