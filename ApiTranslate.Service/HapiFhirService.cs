using ApiTranslate.Domain;
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Entities.Entities;
using ApiTranslate.Domain.Entities.Response;
using ApiTranslate.Domain.Interfaces.Repositories;
using ApiTranslate.Domain.Interfaces.Service;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;

namespace ApiTranslate.Service
{
    public class HapiFhirService : IHapiFhirService
    {
        private readonly IHapiFhirRepository _repo;
        private readonly IHuamiService _huamiService;
        public HapiFhirService(IHapiFhirRepository repo, IHuamiService huamiService)
        {
            _repo = repo;
            _huamiService = huamiService;
        }

        /// <summary>
        /// Returns a patient by Id
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <returns>Models.Patient</returns>
        public async Task<ResultData> GetPatientById(string patientId)
        {
            try
            {
                var result = await _repo.GetPatientById(patientId);
                var serializer = new FhirJsonSerializer(new SerializerSettings()
                {
                    Pretty = true
                });

                var resultJson = serializer.SerializeToString(result);
                return new ResultData(true, resultJson);
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
        public async Task<ResultData> PostObservation(string patientId, DataMiBandRequest request)
        {
            
            try
            {   //busca o paciente
                Patient patient = await _repo.GetPatientById(patientId);

                if (patient == null) return null;

                List<DataMiBandEntity> resultMiBandData = await _huamiService.GetMiBandData(request);
                // ta retornando valores errados. Ha leitura certa, mas na conversao tem algo estranho

                if (resultMiBandData == null) return null;

                foreach (DataMiBandEntity miBandData in resultMiBandData)
                {
                    _repo.PostObservationData(CreateHeartRaceObservationResource(patient, miBandData));
                    _repo.PostObservationData(CreateBurnedCaloriesObservationResource(patient, miBandData));
                    _repo.PostObservationData(CreateStepCountObservationResource(patient, miBandData));
                }

                return new ResultData(true, "Enviado com sucesso");   //repensar a estrutura
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Create a HeartRaceObservation to patient 
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// /// <param name="data">Data MiBand</param>
        /// <returns>Models.Patient</returns>
        private static Observation CreateHeartRaceObservationResource(Patient patient, DataMiBandEntity data)
        {
            Observation obs = new Observation
            {
                Value = new Quantity(Convert.ToDecimal(data.avg_heart_rate), "Hz"),
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> {
                    new Coding {
                        System = "http://loinc.org",
                        Code = "8867-4",
                        Display = "Frequencia Cardiaca Média"
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
                    Start = $"{data.date_time:yyyy-MM-dd}",
                    End = $"{data.date_time:yyyy-MM-dd}",
                }

            };
            return obs;
        }

        /// <summary>
        /// Get all observation to patient 
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="observation">Patient Id</param>
        /// <returns>Models.Patient</returns>
        public async Task<ResultData> GetObservation(string patientId)
        {

            try
            {   
                var result = await _repo.GetObservation(patientId);

                return new ResultData(true, result);  
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Create a BurnedCalories observation to patient 
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// /// <param name="data">Data MiBand</param>
        /// <returns>Models.Patient</returns>
        private static Observation CreateBurnedCaloriesObservationResource(Patient patient, DataMiBandEntity data)
        {

            Observation obs = new Observation
            {
                Value = new Quantity
                {
                    Code = "Kcal",
                    Value = Convert.ToDecimal(data.totalCal),
                    Unit = "kcal",
                    System = "http://unitsofmeasure.org",
                },
                
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> {
                    new Coding {
                         System = "http://loinc.org",
                                Code = "41981-2",
                                Display = "Total Calories Queimadas"
                               }
                    }
                },
                Category = new List<CodeableConcept> {
                    new CodeableConcept {
                        Coding = new List<Coding> {
                            new Coding {
                                System = "http://loinc.org",
                                Code = "41981-2",
                                Display = "Calories burned"
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
                    Start =  $"{data.date_time:yyyy-MM-dd}",
                    End = $"{data.date_time:yyyy-MM-dd}",
                }

            };

            return obs;

        }

        /// <summary>
        /// Create a StepCount observation to patient 
        /// </summary>
        /// <param name="patientId">Patient Id</param>
        /// <param name="data">Data MiBand</param>
        /// <returns>Models.Patient</returns>
        private static Observation CreateStepCountObservationResource(Patient patient, DataMiBandEntity data)
        {

            Observation obs = new Observation
            {
                Value = new Quantity(Convert.ToDecimal(data.totalSteps), "passos"),
                Code = new CodeableConcept
                {
                    Coding = new List<Coding> {
                    new Coding {
                        System = "http://snomed.info/sct",
                        Code = "248263006",
                        Display = "Total de passos"
                    }
                }
                },
                Category = new List<CodeableConcept> {
                    new CodeableConcept {
                        Coding = new List<Coding> {
                            new Coding {
                                System = "http://snomed.info/sct",
                                Code = "248263006",
                                Display = "Total de passos"
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
                    Start = $"{data.date_time:yyyy-MM-dd}",
                    End = $"{data.date_time:yyyy-MM-dd}",
                }

            };

            return obs;

        }

    }
}




//tabela de referencia para construcao de informações:
//Description       |	SNOMED CT  Code	|   LOINC Code	    | UCUM Units
//
//Calories Burned	|   (none)	        |   41981-2	        | kcal; J
//Heart Rate	    |   78564009	    |    8867-4         |{ beats}/ min; / min
//                                          standing: 69001 - 6
//                                          sitting: 69000 - 8
//                                          lying down: 68999 - 2
//Step Count	    |   n/a	            |   55423-8	         |       –
//Sleep Duration	|       248263006	|   n/a	             |  h
//
//

//VALIDAR SE O SEGUNIDE GUIDELINE PODE SER USADO - https://www.nrces.in/preview/ndhm/fhir/r4/StructureDefinition-ObservationPhysicalActivity.html