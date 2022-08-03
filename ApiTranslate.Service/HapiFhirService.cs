using ApiTranslate.Domain;
using ApiTranslate.Domain.Interfaces.Repositories;
using ApiTranslate.Domain.Interfaces.Service;
using Hl7.Fhir.Model;
using System;
using System.Threading.Tasks;

namespace ApiTranslate.Service
{
    public class HapiFhirService : IHapiFhirService
    {
        private readonly IHapiFhirRepository _repo;
        public HapiFhirService(IHapiFhirRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Returns a patient by Id
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>Models.Patient</returns>
        public ResultData GetPatientById(string patientId)
        {
            try
            {
                var result = _repo.GetPatientById(patientId);
                return new ResultData(true, result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

