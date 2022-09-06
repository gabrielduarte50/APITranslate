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
        //criar o seguinte: uma busca pelo nome ou outra info e caso nao ache,
        //crie o novo cliente - https://www.youtube.com/watch?v=aP6DRYH-qOI&t=450s.
        [HttpGet]
        public IActionResult GetPatientById(string patientId) //1190270
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

        [Route("post")]
        [HttpPost]
        public IActionResult PostPatientObservation(string patientId) //1190270
        {
            // tentar enviar como um campo Observation - Bundle
           

            ResultData result = _hapiFhirService.PostObservation(patientId);


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
