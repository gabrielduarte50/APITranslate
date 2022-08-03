using System;
using System.Net;
using System.Threading.Tasks;
using ApiTranslate.Domain;
using Hl7.Fhir.Model;
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiTranslate.Controllers
{
    [Route("/Patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IHapiFhirService _hapiFhirService;
        public PatientController(IHapiFhirService hapiFhirService)
        {
            _hapiFhirService = hapiFhirService;
        }

        [HttpGet]
        public async Task<ActionResult> GetPatientById(string patientId) //30422181
        {

            ResultData result = _hapiFhirService.GetPatientById(patientId);


            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetPatientById(string patientId) //30422181
        {

            ResultData result = _hapiFhirService.GetPatientById(patientId);


            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

    }
}
