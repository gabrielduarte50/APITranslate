
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Entities.Response;
using System.Threading.Tasks;

namespace ApiTranslate.Domain.Interfaces.Apis
{
    public interface IHuamiApi
    {
        Task<CredentialResponse> GetHuamiCredentials();
        Task<DataMiBandResponse> GetHuamiBandData(DataMiBandRequest data, TokenInfo credential);
        Task<DataSportResponse> GetHuamiBandDataSport(DataMiBandRequest data, TokenInfo credential);
    }
}
