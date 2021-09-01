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

        #region General
        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _ctx.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _logger.LogInformation($"Removing an object of type {entity.GetType()} to the context.");
            _ctx.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempting to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _ctx.SaveChangesAsync()) > 0;
        }
        #endregion

        #region Patient
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

        public async Task<Patient> GetPatientByMRNAsync(int mrn, bool includePrognosticInfos = false)
        {
            _logger.LogInformation($"Getting a Patient for {mrn}");

            IQueryable<Patient> query = _ctx.Patients;

            if (includePrognosticInfos)
            {
                query = query
                  .Include(c => c.PrognosticInfos)
                  .ThenInclude(t => t.CellFeatures);
            }

            // Query It
            query = query.Where(c => c.MRN == mrn);

            return await query.FirstOrDefaultAsync();
        }

        #endregion

        #region PrognosticInfos

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
        #endregion

        #region CellularFeatures
        Task<CellFeatures[]> IPatientRepository.GetAllCellFeaturesAsync()
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
