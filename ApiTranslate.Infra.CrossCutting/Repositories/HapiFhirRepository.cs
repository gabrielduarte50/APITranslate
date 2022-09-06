using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using System.Threading.Tasks;
using ApiTranslate.Domain.Interfaces.Repositories;

namespace ApiTranslate.Infra.CrossCutting.Repositories
{
    public class HapiFhirRepository : IHapiFhirRepository 
    {
        private const string _fhirServer = "http://hapi.fhir.org/baseR4";
        // private const string _fhirServer = "http://vonk.fire.ly";
        private readonly FhirClient _client;

        public HapiFhirRepository()
        {
            // Create a client
           
            _client = new FhirClient(_fhirServer);
            _client.Settings.PreferredFormat = ResourceFormat.Json;
            _client.Settings.PreferredReturn = Prefer.ReturnRepresentation;
            _client.Settings.Timeout = 120000; // The timeout is set in milliseconds, with a default of 100000
        }

        public  Patient GetPatientById(string patientId)
        {
            var location = new Uri($"http://hapi.fhir.org/baseR4/Patient/{patientId}");
            //var location = new Uri($"https://vonk.fire.ly/Patient/{patientId}");

            try
            {
                var response = _client.Read<Patient>(location);
                return response;
            }
            catch (Hl7.Fhir.ElementModel.StructuralTypeException structuralTypeException)
            {
                throw structuralTypeException;
            }
        }

        public Observation POstObservationData(Observation obs)
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
