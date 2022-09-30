using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<List<DataMiBandEntity>> GetMiBandData(DataMiBandRequest data) 
        {
            try
            {
                CredentialResponse credential = await _huamiApi.GetHuamiCredentials(data.DeviceId);

                DataMiBandResponse resultData = await _huamiApi.GetHuamiBandData(data, credential.token_info);
                DataSportResponse resultSportData = await _huamiApi.GetHuamiBandDataSport(data, credential.token_info);
               
              
                foreach (Datum e in resultData.data)
                {
                    byte[] dt = Convert.FromBase64String(e.summary);
                    e.summary = Encoding.UTF8.GetString(dt);
                }

                List<DataMiBandEntity> summaryElements = TreatDataResponseToEntity(resultData, resultSportData);

                return summaryElements;
            }
            catch
            {
                return null;
            }
        }

        //Esse método faz os calulos. em algo errado, não ta retornando legal nao
        private static List<DataMiBandEntity> TreatDataResponseToEntity(DataMiBandResponse resultData, DataSportResponse resultSportData) //gerar a entidade final aqui
        {
            List<DataMiBandEntity> data = new List<DataMiBandEntity>();
            DataMiBandEntity element = new DataMiBandEntity();
            SummaryMiBand summaryElements;

            foreach (Datum e in resultData.data)
            {
                element.date_time = DateTime.ParseExact(e.date_time, "yyyy-MM-dd", CultureInfo.InvariantCulture); //date - necessary put this in format long to convert

                //transform summmary 
                summaryElements = System.Text.Json.JsonSerializer.Deserialize<SummaryMiBand>(e.summary); 

                //transform MiBandData into a part of Entity
                element.rhr = summaryElements.slp.rhr > 0 ? summaryElements.slp.rhr : 0; //validar esse 0 aqui
                element.totalSteps = summaryElements.stp.ttl;
                element.totalPai = summaryElements.goal; // summaryElements.pai.tp - - nao mapeou isso aqui nao -  talvez seja o goal

                //sleep data
                var tstInMs = summaryElements.slp.ed * 1000 - summaryElements.slp.st * 1000 - summaryElements.slp.wk * 60 * 1000;
                element.deep_sleep = getFormattedDuration(summaryElements.slp.dp * 60 * 1000);
                element.light_sleep = getFormattedDuration(summaryElements.slp.lt * 60 * 1000);
                element.rem_sleep = getFormattedDuration(summaryElements.slp.dt * 60 * 1000);
                element.sleep_duration = getFormattedDuration(tstInMs);

                //gain data from sport data
                var walk = resultSportData.data.summary.Find(
                    s => (getFormattedTime(s.end_time).ToString() == element.date_time.ToString())
                    && (s.type == 6 || s.type == 8)                   
                );
                if(walk != null)
                {
                    element.walk_distance = walk.dis; //em metros - validar se não é milhas e a necessidade de converter

                    //calories
                    var totalCalRun = walk.calorie; //calorias de corrida
                    element.totalCal = totalCalRun + summaryElements.stp.cal.ToString();

                    //heart
                    element.avg_heart_rate = walk.avg_heart_rate;
                    element.max_heart_rate = walk.max_heart_rate;
                    element.min_heart_rate = walk.min_heart_rate;
                }

                data.Add(element);
            }
            return data;
        }
        private static string getFormattedDuration(int timeInMilliseconds) // isolar em outro service ou shared 
        {   //need find how convert this e pass to formatter date 
            TimeSpan time = TimeSpan.FromMilliseconds(timeInMilliseconds);
            var formattedDuration = time.ToString(@"hh\:mm");

            return formattedDuration;
        }
        private static DateTime getFormattedTime(string time) 
        {    
            var date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(time + "000"));

            return date;
        }

    }

}
//ver sobre entity framework se vai ser preciso ou nao