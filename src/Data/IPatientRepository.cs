using BreastCancerAPI.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data
{
    public interface IPatientRepository
    {
        // General 
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();

        // Patient
        Task<Patient[]> GetAllPatientsAsync(bool includePrognosticInfos = false);
        Task<Patient> GetPatientByMRNAsync(int mrn, bool includePrognosticInfos = false);

        // PrognosticInfo
        Task<PrognosticInfo[]> GetAllPrognosticInfoAsync();
        Task<PrognosticInfo[]> GetAllPrognosticInfoByPatientId(int patient);
        Task<PrognosticInfo[]> GetAllPrognosticInfoByOutcome(string category);

        // CellFeatures
        Task<CellFeatures[]> GetAllCellFeaturesAsync();

    }
}
