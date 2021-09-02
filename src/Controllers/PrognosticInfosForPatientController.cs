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
    [Route("api/patients/{patientid}/prognosticinfosforpatient")]
    [ApiController]
    public class PrognosticInfosForPatientController : ControllerBase
    {
        private readonly PatientContext _context; // Delete Later
        private IPatientRepository _repository;
        private IMapper _mapper;
        private LinkGenerator _linkGenerator;

        public PrognosticInfosForPatientController(PatientContext context,
            IPatientRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _context = context;
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        // GET: api/patients/1/prognosticinfosforpatient/?includeCellFeatures=true
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrognosticInfoModel>>> GetAllPrognosticInfoByPatientId(int patientid, bool includeCellFeatures = false)
        {
            try
            {
                //includeCellFeatures = true to display them by default
                var prognosticInfos = await _repository.GetAllPrognosticInfoByPatientIdAsync(patientid, includeCellFeatures);

                return _mapper.Map<PrognosticInfoModel[]>(prognosticInfos);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        // GET: api/patients/1/prognosticinfosforpatient/200/?includeCellFeatures=true
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PrognosticInfoModel>> GetPrognosticInfoByPatientId(int patientid, int id, bool includeCellFeatures = false)
        {
            try
            {
                //includeCellFeatures = true to display them by default
                var prognosticInfos = await _repository.GetPrognosticInfoByPatientIdAsync(patientid, id, includeCellFeatures);

                return _mapper.Map<PrognosticInfoModel>(prognosticInfos);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        // PUT: api/PrognosticInfos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutPrognosticInfoModel(int patientid, int id, PrognosticInfoModel prognosticInfoModel)
        {
            if (id != prognosticInfoModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(prognosticInfoModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrognosticInfoModelExists(id))
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

        // POST: api/patients/1/prognosticinfosforpatient/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PrognosticInfoModel>> PostPrognosticInfoModel(int patientid,
                                                                                     PrognosticInfoModel prognosticInfoModel)
        {
            try
            {              
                // Validate if a Patient exists
                var patient = await _repository.GetPatientByIdAsync(patientid);
                if (patient == null)
                {
                    return BadRequest("Patient corresponding to PrognosticInfo does not exists");
                }

                // Create entity from Model
                PrognosticInfo prognosticInfo = _mapper.Map<PrognosticInfo>(prognosticInfoModel);

                // Foreign key: Attach Patient to PrognosticInfo before addding to repository
                prognosticInfo.Patient = patient;

                // Add the requested CellFeatures to PrognosticInfo
                if (prognosticInfoModel.CellFeatures == null) return BadRequest("Cell features is required");
                var cellFeatures = await _repository.GetCellFeatureAsync(prognosticInfoModel.CellFeatures.Id);
                if (cellFeatures == null) return BadRequest("cellFeatures information could not be found");
                prognosticInfo.CellFeatures = cellFeatures;

                _repository.Add(prognosticInfo);

                if (await _repository.SaveChangesAsync())
                {
                    // Create the URI to be returned using linkgenerator
                    var location = _linkGenerator.GetPathByAction(HttpContext, "Get",
                                                                   values: new { patientid, id = prognosticInfoModel.Id });

                    if (string.IsNullOrWhiteSpace(location))
                    {
                        //// Hardcoding the string of URI - susceptible to change 
                        location = $"api/patients/{patientid}/prognosticinfosforpatient/{prognosticInfoModel.Id}";
                    }
                    //return the URI and the created object (map back to PrognosticInfoModel)
                    return Created(location, _mapper.Map<PrognosticInfoModel>(prognosticInfo));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }

            // If Save changes failed
            return BadRequest();
        }

        // DELETE: api/patients/1/prognosticinfosforpatient/200
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePrognosticInfoModel(int patientid, int id)
        {
            try
            {
                var prognosticInfo = await _repository.GetPrognosticInfoByPatientIdAsync(patientid, id);
                if (prognosticInfo == null)
                {
                    return NotFound();
                }

                // Delete the entity if found
                _repository.Delete(prognosticInfo);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }

            // If Save changes failed
            return BadRequest();
        }

        private bool PrognosticInfoModelExists(int id)
        {
            return _context.PrognosticInfoModel.Any(e => e.Id == id);
        }
    }
}
