using ApiTranslate.Domain;
using ApiTranslate.Domain.Interfaces.Repositories;
using ApiTranslate.Domain.Interfaces.Service;
using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
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
        
        /// <summary>
        /// Create a observation to patient 
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="observation">Patient Id</param>
        /// <returns>Models.Patient</returns>
        public ResultData PostObservation(string patientId)
        {
            try
            {   //busca o paciente
                Patient patient = _repo.GetPatientById(patientId);

                //cria as observacoes do paciente
                Observation obs = CreateObservationResource(patient);

                //envia as observacoes do paciente
                var result = _repo.POstObservationData(obs);

                return new ResultData(true, result);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static Observation CreateObservationResource(Patient patient)
        {
          
            Observation obs = new Observation
            {
                Value = new Quantity(70, "Hz"),
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> {
                    new Coding {
                        System = "http://loinc.org",
                        Code = "8867-4",
                        Display = "Frequencia Cardiaca"
                    }
                }
                },
                Category = new List<CodeableConcept> {
                    new CodeableConcept {
                        Coding = new List<Coding> {
                            new Coding {
                                System = "http://loinc.org",
                                Code = "8867-4",
                                Display = "Frequencia Cardiaca Média"
                            }
                        }
                    }
                },
                Status = new ObservationStatus(),
                Subject = new ResourceReference
                {
                    Reference = "Patient/" + patient.Id
                },
                Effective = new Period()
                {
                    Start = "2022-05-05",
                    End = "2022-04-03",
                }

            };

         


            return obs;
            
        }
    }
}

