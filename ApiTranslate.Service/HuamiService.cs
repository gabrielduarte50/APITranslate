using System;
using System.Text;
using System.Threading.Tasks;
using ApiTranslate.Domain;
using ApiTranslate.Domain.Entities;
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

        public async Task<ResultData> GetMiBandData(DataMiBandRequest data) //ContaBancariaEntity conta = _mapper.Map<ContaBancariaEntity>(request); o correto é usar Entitys aqui
        {
            try
            {
                DataMiBandResponse result = await _huamiApi.GetHuamiBandData(data);
                
                foreach(Datum e in result.data)
                {
                    byte[] dt = Convert.FromBase64String(e.summary);
                    e.summary = Encoding.UTF8.GetString(dt);
                } // talvez criar um Modelo, pois ficou um json mto confuso

                //aqui vai acontecer o tratamneto dos dados

                return new ResultData(true, result.data);
            }
            catch
            {
                return new ResultData(false, "Não foi possível obter os dados.");
            }
        }
    }

}
//ver sobre entity framework se vai ser preciso ou nao