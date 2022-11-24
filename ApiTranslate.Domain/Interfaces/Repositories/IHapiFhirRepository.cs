using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;
using System.Threading.Tasks;

namespace ApiTranslate.Domain.Interfaces.Repositories
{
   
    public interface IHapiFhirRepository
    {
        Task<Patient> GetPatientById(string patientId);
        Observation PostObservationData(Observation obs);
        Task<List<string>> GetObservation(string patientId);
       
    }
}
