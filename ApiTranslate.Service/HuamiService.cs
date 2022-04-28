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
               
                foreach (Datum e in resultData.data)
                {
                    byte[] dt = Convert.FromBase64String(e.summary);
                    e.summary = Encoding.UTF8.GetString(dt);
                }



                //aqui vai acontecer o tratamneto dos dados

                return new ResultData(true, resultSportData);
            }
            catch
            {
                return new ResultData(false, "Não foi possível obter os dados.");
            }
        }

        //private static DataMiBandEntity TreatDataResponseToEntity(DataMiBandResponse resultData) //gerar o entity final
        //{
        //    List<SummaryMiBand> summaryElements = new List<SummaryMiBand>();
        //    DataMiBandEntity data;

        //    foreach (Datum e in resultData.data)
        //    {
        //        byte[] dt = Convert.FromBase64String(e.summary);
        //        e.summary = Encoding.UTF8.GetString(dt);
        //        summaryElements.Add(System.Text.Json.JsonSerializer.Deserialize<SummaryMiBand>(e.summary)); //tentar colocar isso num automapper antes de cair aqui
        //    }
        //    return data;
        //}
    }

}
//ver sobre entity framework se vai ser preciso ou nao