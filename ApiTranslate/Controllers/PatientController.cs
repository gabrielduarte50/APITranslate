using System;
using System.Net;
using System.Threading.Tasks;
using ApiTranslate.Domain;
using Hl7.Fhir.Model;
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.IO;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace ApiTranslate.Controllers
{
    [Route("/")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IHapiFhirService _hapiFhirService;
        public PatientController(IHapiFhirService hapiFhirService)
        {
            _hapiFhirService = hapiFhirService;
        }

        [Route("Patient")]
        [HttpGet]
        public async Task<IActionResult> GetPatientById(string patientId) 
        {

            ResultData result = await _hapiFhirService.GetPatientById(patientId);


            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }


        [Route("Observation")]
        [HttpPost]
        public async Task<IActionResult> PostPatientObservation(string patientId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate) 
        {
            DataMiBandRequest request = new DataMiBandRequest
            {
                startDate = startDate,
                endDate = endDate
            };

            ResultData result = await _hapiFhirService.PostObservation(patientId, request);

                            
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
            
        }

        [Route("Observations")]
        [HttpGet]
        public async Task<IActionResult> GetAllObservations(string patientId) 
        {

            ResultData result = await _hapiFhirService.GetObservation(patientId);


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
