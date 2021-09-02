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
        Task<Patient> GetPatientByIdAsync(int id, bool includePrognosticInfos = false);

        // PrognosticInfo
        Task<PrognosticInfo[]> GetAllPrognosticInfoAsync(bool includeCellFeatures = false);
        Task<PrognosticInfo[]> GetAllPrognosticInfoByOutcomeAsync(string outcome, bool includeCellFeatures = false);
        Task<PrognosticInfo[]> GetAllPrognosticInfoByPatientIdAsync(int patientId, bool includeCellFeatures = false);
        Task<PrognosticInfo> GetPrognosticInfoByPatientIdAsync(int patientId, int id, bool includeCellFeatures = false);
        

        // CellFeatures
        Task<CellFeatures[]> GetAllCellFeaturesAsync();

    }
}
