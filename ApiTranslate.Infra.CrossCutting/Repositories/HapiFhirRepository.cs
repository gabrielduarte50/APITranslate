using System;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.Threading.Tasks;
using ApiTranslate.Domain.Interfaces.Repositories;
using Newtonsoft.Json;
using Hl7.Fhir.Serialization;

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
            _client.Settings.UseFormatParameter = true;
            _client.Settings.Timeout = 120000; // The timeout is set in milliseconds, with a default of 100000
        }

        public async Task<List<string>> GetObservation(string patientId)
        {
            var serializer = new FhirJsonSerializer(new SerializerSettings()
            {
                Pretty = true
            });

            try
            {
                List<string> result = new List<String>();
                var searchParameters = new SearchParams();
                searchParameters.Parameters.Add(new Tuple<string, string>("patient", patientId));
                var responsBundle = await _client.SearchAsync<Observation>(searchParameters).ConfigureAwait(false);
                foreach (var entry in responsBundle.Entry)
                {
                    var observation = (Observation)entry.Resource;
                    result.Add(serializer.SerializeToString(observation));
                  
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
                var response = await _client.ReadAsync<Patient>(location);
            
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
