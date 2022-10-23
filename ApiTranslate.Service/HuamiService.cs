using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
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

        private static List<DataMiBandEntity> TreatDataResponseToEntity(DataMiBandResponse resultData, DataSportResponse resultSportData) 
        {
            List<DataMiBandEntity> data = new List<DataMiBandEntity>();
            DataMiBandEntity element = new DataMiBandEntity();
            SummaryMiBand summaryElements;

            foreach (Datum e in resultData.data)
            {
                element.date_time = DateTime.ParseExact(e.date_time, "yyyy-MM-dd", CultureInfo.InvariantCulture); 

                //transform summmary 
                summaryElements = System.Text.Json.JsonSerializer.Deserialize<SummaryMiBand>(e.summary); 

                //transform MiBandData into a part of Entity
                element.rhr = summaryElements.slp.rhr > 0 ? summaryElements.slp.rhr : 0; 
                element.totalSteps = summaryElements.stp.ttl;
                element.totalPai = summaryElements.goal; 

                //sleep data
                var tstInMs = summaryElements.slp.ed * 1000 - summaryElements.slp.st * 1000 - summaryElements.slp.wk * 60 * 1000;
                element.deep_sleep = getFormattedDuration(summaryElements.slp.dp * 60 * 1000);
                element.light_sleep = getFormattedDuration(summaryElements.slp.lt * 60 * 1000);
                element.rem_sleep = getFormattedDuration(summaryElements.slp.dt * 60 * 1000);
                element.sleep_duration = getFormattedDuration(tstInMs);

                //gain data from sport data - validar se o type é ou nao importante
                var dateSearch = element.date_time.ToShortDateString().ToString();
                var walk = resultSportData.data.summary.Find(
                    s => (getFormattedTime(s.end_time) == dateSearch)
                );

                if(walk != null) //validar se a caloria do summary realmente ta na mesma data da de corrida
                {
                    element.walk_distance = walk.dis; 

                    //calories
                    var totalCalRun = walk.calorie; //calorias de corrida
                    element.totalCal = Somar(totalCalRun, summaryElements.stp.cal.ToString()); 

                    //heart
                    element.avg_heart_rate = walk.avg_heart_rate;
                    element.max_heart_rate = walk.max_heart_rate;
                    element.min_heart_rate = walk.min_heart_rate;

                    //clear walk
                    walk = null;
                }

                data.Add(new DataMiBandEntity()
                {
                    date_time = element.date_time, 
                    rhr = element.rhr, 
                    totalSteps = element.totalSteps,
                    totalPai = element.totalPai,
                    deep_sleep = element.deep_sleep,
                    light_sleep = element.light_sleep, 
                    rem_sleep = element.rem_sleep ,
                    sleep_duration = element.sleep_duration,
                    totalCal = element.totalCal,
                    avg_heart_rate = element.avg_heart_rate
                });

             }
            return data;
        }
        private static string getFormattedDuration(int timeInMilliseconds) 
        {    
            TimeSpan time = TimeSpan.FromMilliseconds(timeInMilliseconds);
            var formattedDuration = time.ToString(@"hh\:mm");

            return formattedDuration;
        }
        private static string getFormattedTime(string time) 
        {
            DateTime date = (new DateTime(1970, 1, 1)).AddMilliseconds(double.Parse(time + "000"));
            var d = date.ToShortDateString().ToString();

            return d;
        }
        private static string Somar(string numA, string numB)
        {
            var bigA = float.Parse(numA, CultureInfo.InvariantCulture.NumberFormat); 
            var bigB = float.Parse(numB, CultureInfo.InvariantCulture.NumberFormat);

            return (bigA + bigB).ToString();
        }

    }

}
