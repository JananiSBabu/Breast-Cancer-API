using BreastCancerAPI.Data;
using BreastCancerAPI.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BreastCancerAPI.Controllers
{
    [Route("api/patients/{patientid}/ClinicalInfos")]
    [ApiController]
    public class ClinicalInfosController : ControllerBase
    {
        private readonly IClinicalInfoRepository _clinicalInfoRepository;
        //private readonly IDataRepository<ClinicalInfo> _clinicalInfoRepository2;

        public ClinicalInfosController(IClinicalInfoRepository clinicalInfoRepository)
        {
            _clinicalInfoRepository = clinicalInfoRepository;
            //_clinicalInfoRepository2 = clinicalInfoRepository2;
        }

        // GET: api/<ClinicalInfosController>
        [ProducesResponseType(typeof(IReadOnlyList<ClinicalInfo>), 200)]
        [HttpGet()]
        public async Task<IActionResult> GetAllClinicalInfoAsync()
        {
            var clinicalInfo = await _clinicalInfoRepository.GetAllAsync();
            return Ok(clinicalInfo);
        }

        // GET api/<ClinicalInfosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClinicalInfo>> Get(string id)
        {
            
            try
            {
                // results are "entities" themselves -> Patient
                ClinicalInfo clinicalInfo = await _clinicalInfoRepository.GetAsync(id);

                if (clinicalInfo == null)
                {
                    return NotFound();
                }
                // Todo: Map entity to model - AM

                return clinicalInfo;

            }
            catch (Exception e)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database failure");
            }
        }

        // POST api/<ClinicalInfosController>
        [HttpPost]
        public async Task<ActionResult<ClinicalInfo>> Post(int patientid, [FromBody] ClinicalInfo entity)
        {
            if (ModelState.IsValid)
            {
                entity.Id = Guid.NewGuid().ToString();
                ClinicalInfo clinicalInfo = await _clinicalInfoRepository.AddAsync(entity);
            }

            return Ok();
        }

        
    }
}
