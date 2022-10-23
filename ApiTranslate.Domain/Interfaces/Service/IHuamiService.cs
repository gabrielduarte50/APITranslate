using ApiTranslate.Domain.Entities;
using System.Threading.Tasks;
using System;
using ApiTranslate.Domain.Entities.Entities;
using System.Collections.Generic;

namespace ApiTranslate.Domain.Interfaces.Service
{
    public interface IHuamiService
    {
        Task<List<DataMiBandEntity>> GetMiBandData(DataMiBandRequest data);
    }
}

