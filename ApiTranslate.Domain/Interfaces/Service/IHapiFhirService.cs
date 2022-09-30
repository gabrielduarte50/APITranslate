using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Hl7.Fhir.Model;
using ApiTranslate.Domain.Entities;

namespace ApiTranslate.Domain.Interfaces.Service
{
    public interface IHapiFhirService
    {
        ResultData GetPatientById(string patientId);
        Task<ResultData> PostObservation(string patientId, DataMiBandRequest request);
    }
}
