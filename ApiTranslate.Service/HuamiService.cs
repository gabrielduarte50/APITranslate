using System;
using System.Threading.Tasks;
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

        public async Task<DataMiBandResponse> GetMiBandData(DataMiBandRequest data) //ContaBancariaEntity conta = _mapper.Map<ContaBancariaEntity>(request); o correto é usar Entitys aqui
        {
            try
            {
                var result = await _huamiApi.GetHuamiBandData(data);

                return result;
            }
            catch
            {
                throw;
            }
        }
    }

}
//ver sobre entity framework se vai ser preciso ou nao