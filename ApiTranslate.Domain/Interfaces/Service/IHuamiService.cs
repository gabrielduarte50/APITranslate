using ApiTranslate.Domain.Entities;
using System.Threading.Tasks;
using System;

namespace ApiTranslate.Domain.Interfaces.Service
{
    public interface IHuamiService
    {
        Task<ResultData> GetMiBandData(DataMiBandRequest data);
    }
}

