using System;
using System.Threading.Tasks;
using ApiTranslate.Domain;
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Entities.Response;
using ApiTranslate.Domain.Interfaces.Apis;
using ApiTranslate.Domain.Interfaces.Service;

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

                //aqui vai acontecer o tratamneto dos dados

                return new ResultData( true, result);
            }
            catch
            {
                return new ResultData(false, "Não foi possível obter os dados.");
            }
        }
    }

}
//ver sobre entity framework se vai ser preciso ou nao