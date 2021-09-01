using BreastCancerAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientContext _ctx;

        public PatientRepository(PatientContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<Patient> GetAllPatients()
        {
            return _ctx.Patients
                        .OrderBy(p => p.MRN)
                        .ToList();
        }

        void IPatientRepository.AddEntity(object model)
        {
            throw new NotImplementedException();
        }

        IEnumerable<CellFeatures> IPatientRepository.GetAllCellFeatures()
        {
            throw new NotImplementedException();
        }

        IEnumerable<PrognosticInfo> IPatientRepository.GetAllPrognosticInfo()
        {
            throw new NotImplementedException();
        }

        IEnumerable<PrognosticInfo> IPatientRepository.GetAllPrognosticInfoByOutcome(string category)
        {
            throw new NotImplementedException();
        }

        IEnumerable<PrognosticInfo> IPatientRepository.GetAllPrognosticInfoByPatientId(int patient)
        {
            throw new NotImplementedException();
        }

        bool IPatientRepository.SaveAll()
        {
            throw new NotImplementedException();
        }
    }
}
