using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Hl7.Fhir.Model;

namespace ApiTranslate.Domain.Interfaces.Service
{
    public interface IHapiFhirService
    {
        ResultData GetPatientById(string patientId);
        ResultData PostObservation(string patientId);
    }
}
