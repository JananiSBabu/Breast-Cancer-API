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
    [ApiVersion("1")]
    [Route("api/patients/{patientid}/prognosticinfosforpatient")]
    [ApiController]
    public class PrognosticInfosForPatientController : ControllerBase
    {
        private IPatientRepository _repository;
        private IMapper _mapper;
        private LinkGenerator _linkGenerator;

        public PrognosticInfosForPatientController(IPatientRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
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
        public async Task<IActionResult> PutPrognosticInfo(int patientid, int id, PrognosticInfoModel prognosticInfoModel)
        {
            if (id != prognosticInfoModel.Id)
            {
                return BadRequest();
            }

            try
            {
                var oldPrognosticInfo = await _repository.GetPrognosticInfoByPatientIdAsync(patientid, id, true);
                if (oldPrognosticInfo == null) return NotFound("Prognostic Info could not be found");

                //B usecase: if cellFeatures is present in request, then overwrite PrognosticInfo.cellFeatures manually
                // BEFORE using mapper. (mapper designed to prevent accidental overwrites)
                if (prognosticInfoModel.CellFeatures != null)
                {
                    var cellFeatures = await _repository.GetCellFeatureAsync(prognosticInfoModel.CellFeatures.Id);
                    if (cellFeatures != null)
                    {
                        oldPrognosticInfo.CellFeatures = cellFeatures;
                    }
                }

                // Map changes from new model to the oldPrognosticInfo
                _mapper.Map(prognosticInfoModel, oldPrognosticInfo);

                if (await _repository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<PrognosticInfoModel>(oldPrognosticInfo));
                }
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
            return BadRequest("Failed to save changes: Talk");
        }

        // POST: api/patients/1/prognosticinfosforpatient/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PrognosticInfoModel>> PostPrognosticInfo(int patientid,
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
        public async Task<IActionResult> DeletePrognosticInfo(int patientid, int id)
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

    }
}
