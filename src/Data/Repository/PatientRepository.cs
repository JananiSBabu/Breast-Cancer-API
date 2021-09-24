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

        public async Task<Patient> GetPatientByIdAsync(int id, bool includePrognosticInfos = false)
        {
            _logger.LogInformation($"Getting a Patient for {id}");

            IQueryable<Patient> query = _ctx.Patients;

            if (includePrognosticInfos)
            {
                query = query
                  .Include(c => c.PrognosticInfos)
                  .ThenInclude(t => t.CellFeatures);
            }

            // Query It
            query = query.Where(c => c.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        #endregion

        #region PrognosticInfos
        public async Task<PrognosticInfo[]> GetAllPrognosticInfoAsync(bool includeCellFeatures = false)
        {
            _logger.LogInformation($"Getting all PrognosticInfos");

            IQueryable<PrognosticInfo> query = _ctx.PrognosticInfos;

            if (includeCellFeatures)
            {
                query = query
                  .Include(t => t.CellFeatures);
            }

            // Order It
            query = query.OrderByDescending(c => c.Outcome);

            return await query.ToArrayAsync();
        }

        public async Task<PrognosticInfo[]> GetAllPrognosticInfoByOutcomeAsync(string outcome, bool includeCellFeatures = false)
        {
            _logger.LogInformation($"Getting all PrognosticInfos");

            IQueryable<PrognosticInfo> query = _ctx.PrognosticInfos;

            if (includeCellFeatures)
            {
                query = query
                  .Include(t => t.CellFeatures);
            }

            // Order It
            query = query.Where(c => c.Outcome == outcome);

            return await query.ToArrayAsync();
        }

        public async Task<PrognosticInfo[]> GetAllPrognosticInfoByPatientIdAsync(int patientId, bool includeCellFeatures = false)
        {
            _logger.LogInformation($"Getting all PrognosticInfos for patientId {patientId}");

            IQueryable<PrognosticInfo> query = _ctx.PrognosticInfos;

            if (includeCellFeatures)
            {
                query = query
                  .Include(t => t.CellFeatures);
            }

            // Add query
            query = query
                    .Where(t => t.Patient.Id == patientId)
                    .OrderByDescending(c => c.Outcome);

            return await query.ToArrayAsync();            
        }

        public async Task<PrognosticInfo> GetPrognosticInfoByPatientIdAsync(int patientId, int id, bool includeCellFeatures = false)
        {
            _logger.LogInformation($"Getting PrognosticInfos {id} for patientId {patientId}");

            IQueryable<PrognosticInfo> query = _ctx.PrognosticInfos;

            if (includeCellFeatures)
            {
                query = query
                  .Include(t => t.CellFeatures);
            }

            // Add query
            query = query
                    .Where(t => t.Id == id && t.Patient.Id == patientId);

            return await query.FirstOrDefaultAsync();
        }

        #endregion

        #region CellularFeatures
        public async Task<CellFeatures> GetCellFeatureAsync(int id)
        {
            _logger.LogInformation($"Getting Cell Features");

            var query = _ctx.CellFeatures
              .Where(t => t.Id == id);

            return await query.FirstOrDefaultAsync();
        }
        #endregion

    }
}
