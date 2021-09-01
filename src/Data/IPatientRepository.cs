using BreastCancerAPI.Data.Entities;
using System.Collections.Generic;

namespace BreastCancerAPI.Data
{
    public interface IPatientRepository
    {
        IEnumerable<Patient> GetAllPatients();
        IEnumerable<PrognosticInfo> GetAllPrognosticInfo();

        IEnumerable<PrognosticInfo> GetAllPrognosticInfoByPatientId(int patient);

        IEnumerable<PrognosticInfo> GetAllPrognosticInfoByOutcome(string category);
        

        IEnumerable<CellFeatures> GetAllCellFeatures();
        void AddEntity(object model);

        bool SaveAll();
    }
}
