using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreastCancerAPI.Data;
using BreastCancerAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using BreastCancerAPI.Data.Entities;

namespace BreastCancerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private PatientContext _context; // To be deleted
        private IPatientRepository _repository;
        private IMapper _mapper;
        private LinkGenerator _linkGenerator;

        public PatientsController(PatientContext context,
            IPatientRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientModel>>> GetPatients(bool includePrognosticInfos = false)
        {
            try
            {
                // results are "entities" themselves -> Patient
                var results = await _repository.GetAllPatientsAsync(includePrognosticInfos);

                // map a model to the entities
                // here AM returns list of PatientModels
                PatientModel[] models = _mapper.Map<PatientModel[]>(results);

                return models;
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        // GET: api/Patients/5
        [HttpGet("{mrn}")]
        public async Task<ActionResult<PatientModel>> GetPatient(int mrn, bool includePrognosticInfos = false)
        {
            try
            {
                // results are "entities" themselves -> Patient
                var result = await _repository.GetPatientByMRNAsync(mrn, includePrognosticInfos);

                if (result == null)
                {
                    return NotFound();
                }

                // map a model to the entities
                // here AM returns a single model 
                PatientModel model = _mapper.Map<PatientModel>(result);

                return model;

            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        // PUT: api/Patients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, PatientModel patientModel)
        {
            if (id != patientModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(patientModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Patients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PatientModel>> PostPatient(PatientModel patientModel, bool includePrognosticInfos = false)
        {
            // In Post, get input as PatientModel and save it as "Patient".[Reverse of Get]

            // Validate the uniqueness of the moniker
            var existing = await _repository.GetPatientByMRNAsync(patientModel.MRN, includePrognosticInfos);
            if (existing != null)
            {
                return BadRequest("MRN is already existing");
            }

            try
            {
                var s1 = new { mrn = patientModel.MRN };

                // Create the URI to be returned using linkgenerator
                var location = _linkGenerator.GetPathByAction("Get",
                   "Patients", values: new { mrn = patientModel.MRN });

                if (string.IsNullOrWhiteSpace(location))
                {
                    //// Hardcoding the string of URI - susceptible to change 
                    location = $"/api/camps/{patientModel.MRN}";
                }

                // Create entity from Model
                Patient patient = _mapper.Map<Patient>(patientModel);

                _repository.Add(patient);

                if (await _repository.SaveChangesAsync())
                {
                    //return the URI and the created object (map back to campModel)
                    return Created(location, _mapper.Map<PatientModel>(patient));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }

            // If Save changes failed
            return BadRequest();
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patientModel = await _context.PatientModel.FindAsync(id);
            if (patientModel == null)
            {
                return NotFound();
            }

            _context.PatientModel.Remove(patientModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.PatientModel.Any(e => e.Id == id);
        }
    }
}
