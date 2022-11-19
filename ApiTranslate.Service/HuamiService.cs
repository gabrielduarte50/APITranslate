using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
                CredentialResponse credential = await _huamiApi.GetHuamiCredentials();

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

                //gain data from sport data - validar se o type é ou nao importante
                var dateSearch = element.date_time.ToShortDateString().ToString();

                element.totalCal = summaryElements.stp.cal.ToString();

                var walkList = resultSportData.data.summary.FindAll(
                    s => (getFormattedTime(s.end_time) == dateSearch)
                );

                if(walkList.Count() != 0) 
                {
                    //heart
                    element.avg_heart_rate = (walkList.Sum(w => float.Parse(w.avg_heart_rate, CultureInfo.InvariantCulture.NumberFormat)) /walkList.Count()).ToString(); 

                }

                data.Add(new DataMiBandEntity()
                {
                    date_time = element.date_time, 
                    rhr = element.rhr, 
                    totalSteps = element.totalSteps,
                    totalPai = element.totalPai,
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
