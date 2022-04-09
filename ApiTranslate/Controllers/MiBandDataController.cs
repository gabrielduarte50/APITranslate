using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiTranslate.Controllers
{
    public class MiBandDataController : ControllerBase
    {
        private readonly IHuamiService _huamiService;

        public MiBandDataController(IHuamiService huamiService)
        {
            _huamiService = huamiService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllData(DataMiBandRequest data)
        {
            var result = await _huamiService.GetMiBandData(data);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }
    }
}
