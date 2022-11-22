using Backend.Infra;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("/api")]
    public class LogController : ControllerBase
    {
        private ILogRequestService _logRequestService;

        public LogController(ILogRequestService logRequestService)
        {
            _logRequestService = logRequestService;
        }

        [HttpGet]
        [Route("requests/{fromMillisecond}/{toMillisecond}")]
        public async Task<int> GetRequestsByTimespan([FromRoute] long fromMillisecond, [FromRoute] long toMillisecond)
        {
            var result = await _logRequestService.GetRequestsByTimespan(fromMillisecond, toMillisecond);
            return result;
        }

        [HttpGet]
        [Route("requests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var result = await _logRequestService.GetAllRequests();
            return Ok(result);
        }

        [HttpGet]
        [Route("millisecondsnow")]
        public IActionResult GetCurrentMilliseconds()
        {
            var result = _logRequestService.GetCurrentMilliseconds();
            return Ok(result);
        }

        [HttpGet]
        [Route("resources")]
        public IActionResult GetResourcesInfo()
        {
            var mem = _logRequestService.GetMemory();
            var cpu = _logRequestService.GetCpu();
            var resourcesInfo = new ResourcesInfo { Cpu = cpu, Memory = mem };

            return Ok(resourcesInfo);
        }
    }
}