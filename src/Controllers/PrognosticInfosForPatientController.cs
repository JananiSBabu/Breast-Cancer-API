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

        // GET: api/PrognosticInfos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrognosticInfoModel>>> GetAllPrognosticInfoByPatientId(int patientid, bool includeCellFeatures = false)
        {
            try
            {
                //includeSpeakers = true to display them by default
                var prognosticInfos = await _repository.GetAllPrognosticInfoByPatientIdAsync(patientid, includeCellFeatures);

                return _mapper.Map<PrognosticInfoModel[]>(prognosticInfos);
            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        // GET: api/PrognosticInfos/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PrognosticInfoModel>> GetPrognosticInfoByPatientId(int patientid, int id, bool includeCellFeatures = false)
        {
            try
            {
                //includeSpeakers = true to display them by default
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

        // POST: api/PrognosticInfos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PrognosticInfoModel>> PostPrognosticInfoModel(int patientid, PrognosticInfoModel prognosticInfoModel)
        {
            _context.PrognosticInfoModel.Add(prognosticInfoModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrognosticInfoModel", new { id = prognosticInfoModel.Id }, prognosticInfoModel);
        }

        // DELETE: api/PrognosticInfos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePrognosticInfoModel(int patientid, int id)
        {
            var prognosticInfoModel = await _context.PrognosticInfoModel.FindAsync(id);
            if (prognosticInfoModel == null)
            {
                return NotFound();
            }

            _context.PrognosticInfoModel.Remove(prognosticInfoModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PrognosticInfoModelExists(int id)
        {
            return _context.PrognosticInfoModel.Any(e => e.Id == id);
        }
    }
}
