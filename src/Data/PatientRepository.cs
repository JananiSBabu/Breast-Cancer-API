using BreastCancerAPI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreastCancerAPI.Data
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientContext _ctx;
        private readonly ILogger<PatientRepository> _logger;

        public PatientRepository(PatientContext ctx, ILogger<PatientRepository> logger)
        {
            _ctx = ctx;
            _logger = logger;
        }

        public async Task<Patient[]> GetAllPatientsAsync(bool includePrognosticInfos = false)
        {
            _logger.LogInformation($"Getting all Patients");

            IQueryable<Patient> query = _ctx.Patients;

            if (includePrognosticInfos)
            {
                query = query
                  .Include(c => c.PrognosticInfos)
                  .ThenInclude(t => t.CellFeatures);
            }

            // Order It
            query = query.OrderByDescending(c => c.MRN);

            return await query.ToArrayAsync();
        }

        void IPatientRepository.Add<T>(T entity)
        {
            throw new NotImplementedException();
        }

        void IPatientRepository.Delete<T>(T entity)
        {
            throw new NotImplementedException();
        }

        Task<CellFeatures[]> IPatientRepository.GetAllCellFeaturesAsync()
        {
            throw new NotImplementedException();
        }

        Task<PrognosticInfo[]> IPatientRepository.GetAllPrognosticInfoAsync()
        {
            throw new NotImplementedException();
        }

        Task<PrognosticInfo[]> IPatientRepository.GetAllPrognosticInfoByOutcome(string category)
        {
            throw new NotImplementedException();
        }

        Task<PrognosticInfo[]> IPatientRepository.GetAllPrognosticInfoByPatientId(int patient)
        {
            throw new NotImplementedException();
        }

        Task<bool> IPatientRepository.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
