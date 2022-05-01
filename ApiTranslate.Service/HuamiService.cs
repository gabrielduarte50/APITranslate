using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiTranslate.Domain;
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Entities.Entities;
using ApiTranslate.Domain.Entities.Response;
using ApiTranslate.Domain.Interfaces.Apis;
using ApiTranslate.Domain.Interfaces.Service;
using Google.Apis.Util;

namespace ApiTranslate.Service
{
    public class HuamiService : IHuamiService
    {
        private readonly IHuamiApi _huamiApi;
        public HuamiService(IHuamiApi huamiApi)
        {
            _huamiApi = huamiApi;
        }

        public async Task<ResultData> GetMiBandData(DataMiBandRequest data) 
        {
            try
            {
                CredentialResponse credential = await _huamiApi.GetHuamiCredentials(data.DeviceId);

                DataMiBandResponse resultData = await _huamiApi.GetHuamiBandData(data, credential.token_info);
                DataSportResponse resultSportData = await _huamiApi.GetHuamiBandDataSport(data, credential.token_info);
               
              
                foreach (Datum e in resultData.data) //put summary in Json formatter
                {
                    byte[] dt = Convert.FromBase64String(e.summary);
                    e.summary = Encoding.UTF8.GetString(dt);
                }


                List<DataMiBandEntity> summaryElements = TreatDataResponseToEntity(resultData, resultSportData);
                



                //aqui vai acontecer o tratamneto dos dados

                return new ResultData(true, resultSportData);
            }
            catch
            {
                return new ResultData(false, "Não foi possível obter os dados.");
            }
        }

        private static List<DataMiBandEntity> TreatDataResponseToEntity(DataMiBandResponse resultData, DataSportResponse resultSportData) //gerar a entidade final aqui
        {
            List<DataMiBandEntity> data = new List<DataMiBandEntity>();
            DataMiBandEntity element = new DataMiBandEntity();
            SummaryMiBand summaryElements = new SummaryMiBand();

            foreach (Datum e in resultData.data)
            {
                element.date_time = new DateTime(e.date_time); //date - necessary put this in format long to convert

                //transform summmary 
                byte[] dt = Convert.FromBase64String(e.summary);
                e.summary = Encoding.UTF8.GetString(dt);
                summaryElements = System.Text.Json.JsonSerializer.Deserialize<SummaryMiBand>(e.summary); 

                //transform MiBandData into a part of Entity
                element.rhr = summaryElements.slp.rhr > 0 ? summaryElements.slp.rhr : 0; //validar esse 0 aqui
                element.totalSteps = summaryElements.stp.ttl;
                element.totalPai = summaryElements.goal; // summaryElements.pai.tp - - nao mapeou isso aqui nao -  talvez seja o goal
                element.deep_sleep = getFormattedDuration(summaryElements.slp.dp * 60 * 1000);
                element.light_sleep = getFormattedDuration(summaryElements.slp.lt * 60 * 1000);
                element.rem_sleep = getFormattedDuration(summaryElements.slp.dt * 60 * 1000);

            }
            return data;
        }
        private static string getFormattedDuration(int timeInMilliseconds) // isolar em outro service ou shared 
        {   //need find how convert this e pass to formatter date 
            var hours = Math.Floor(timeInMilliseconds / (1000 * 60 * 60) % 24).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false});
            var minutes = Math.floor(timeInMilliseconds / (1000 * 60) % 60).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false});
            var formattedDuration = hours + ':' + minutes;

            return formattedDuration;
        }


    }

}
//ver sobre entity framework se vai ser preciso ou nao