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
        public async Task<ActionResult> GetAllData(string deviceId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate) // device-id - "FE:22:50:4B:49:D2"
        {
            DataMiBandRequest data = new DataMiBandRequest
            {
                DeviceId = deviceId,
                startDate = startDate,
                endDate = endDate
            };                   
           
            var resultMiBand = await _huamiService.GetMiBandData(data);

            ResultData result = new ResultData 
            {
                Success = true,
                Data = resultMiBand
            };

            if (result.Success)
            {
                return Ok(result) ;
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }
    }
}
