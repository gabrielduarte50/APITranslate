using System;
using System.Net;
using System.Threading.Tasks;
using ApiTranslate.Domain;
using ApiTranslate.Domain.Entities;
using ApiTranslate.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace ApiTranslate.Controllers
{
    [Route("/miband")]
    [ApiController]
    public class MiBandDataController : ControllerBase
    {
        private readonly IHuamiService _huamiService;

        public MiBandDataController(IHuamiService huamiService)
        {
            _huamiService = huamiService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllData(string deviceId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            DataMiBandRequest data = new DataMiBandRequest
            {
                DeviceId = deviceId,
                startDate = startDate,
                endDate = endDate
            };
            ResultData result = await _huamiService.GetMiBandData(data);

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
