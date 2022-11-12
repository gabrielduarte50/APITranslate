﻿using System;
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
   // [Authorize]
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
        public async Task<IActionResult> GetPatientById(string patientId) //1190270
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
        [HttpGet]
        public async Task<IActionResult> GetPatientObservation(string patientId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate) //1190270
        {
            DataMiBandRequest request = new DataMiBandRequest
            {
                startDate = startDate,
                endDate = endDate
            };

            ResultData resultPostData = await _hapiFhirService.PostObservation(patientId, request);

            if (resultPostData.Success)
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
            } else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, resultPostData.Data);
            }
        }

    }
}
