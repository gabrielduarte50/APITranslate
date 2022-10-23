using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using System.Threading.Tasks;
using ApiTranslate.Domain.Interfaces.Repositories;
using System.Linq;
using ApiTranslate.Domain.Entities.Response;
using Newtonsoft.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using static Google.Apis.Requests.BatchRequest;

namespace ApiTranslate.Infra.CrossCutting.Repositories
{
    public class HapiFhirRepository : IHapiFhirRepository 
    {
        private const string _fhirServer = "http://hapi.fhir.org/baseR4";
        private readonly FhirClient _client;

        public HapiFhirRepository()
        {
            // Create a client
           
            _client = new FhirClient(_fhirServer);
            _client.Settings.PreferredFormat = ResourceFormat.Json;
            _client.Settings.PreferredReturn = Prefer.ReturnRepresentation;
            _client.Settings.Timeout = 120000; // The timeout is set in milliseconds, with a default of 100000
        }

        public async Task<List<Observation>> GetObservation(string patientId)
        {
            try
            {
                List<Observation> result = new List<Observation>();
                var searchParameters = new SearchParams();
                searchParameters.Parameters.Add(new Tuple<string, string>("patient", patientId));
                var responsBundle = await _client.SearchAsync<Observation>(searchParameters).ConfigureAwait(false);
                foreach (var entry in responsBundle.Entry)
                {
                    var observation = (Observation)entry.Resource;
                    result.Add(observation);
                  
                }
                return result;

            }
            catch (Hl7.Fhir.ElementModel.StructuralTypeException structuralTypeException)
            {
                throw structuralTypeException;
            }
        }

        public async Task<Patient> GetPatientById(string patientId)
        {
            var location = new Uri($"https://hapi.fhir.org/baseR4/Patient/{patientId}");

            try
            {
                Patient response = await _client.ReadAsync<Patient>(location);

                return response;
            }
            catch (Hl7.Fhir.ElementModel.StructuralTypeException structuralTypeException)
            {
                throw structuralTypeException;
            }
        }

        public Observation PostObservationData(Observation obs)
        {
            try
            {
                var response = _client.Create<Observation>(obs);
                return response;
            }
            catch (Hl7.Fhir.ElementModel.StructuralTypeException structuralTypeException)
            {
                throw structuralTypeException;
            }
        }
    }
}
