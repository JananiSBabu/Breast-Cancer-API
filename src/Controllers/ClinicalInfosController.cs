using BreastCancerAPI.Data;
using BreastCancerAPI.Data.Entities;
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
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ClinicalInfosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClinicalInfosController>
        [HttpPost]
        public async Task<ActionResult<ClinicalInfo>> Post(int patientid, [FromBody] ClinicalInfo entity)
        {
            if (ModelState.IsValid)
            {
                entity.Id = Guid.NewGuid().ToString();
                await _clinicalInfoRepository.AddAsync(entity);
                return RedirectToAction("Index");
            }

            return Ok();
        }

        // PUT api/<ClinicalInfosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClinicalInfosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
