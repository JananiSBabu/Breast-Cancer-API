using BreastCancerAPI.Data.Entities;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data
{
    public interface IClinicalInfoRepository : IDataRepository<ClinicalInfo>
    {
        Task<ClinicalInfo> GetExistingReservationByCarIdAsync(int clinicalInfoId);
    }
}