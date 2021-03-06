
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Entities.Response;
using System.Threading.Tasks;

namespace ApiTranslate.Domain.Interfaces.Apis
{
    public interface IHuamiApi
    {
        Task<CredentialResponse> GetHuamiCredentials(string deviceId);
        Task<DataMiBandResponse> GetHuamiBandData(DataMiBandRequest data);
    }
}
