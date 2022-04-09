using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Entities.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiTranslate.Domain.Interfaces.Service
{
    public interface IHuamiService
    {
        Task<DataMiBandResponse> GetMiBandData(DataMiBandRequest data);
    }
}

