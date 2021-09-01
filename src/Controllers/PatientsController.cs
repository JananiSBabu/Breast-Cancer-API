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
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientModel>> GetPatient(int id)
        {
            var patientModel = await _context.PatientModel.FindAsync(id);

            if (patientModel == null)
            {
                return NotFound();
            }

            return patientModel;
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
        public async Task<ActionResult<PatientModel>> PostPatient(PatientModel patientModel)
        {
            _context.PatientModel.Add(patientModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatientModel", new { id = patientModel.Id }, patientModel);
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
