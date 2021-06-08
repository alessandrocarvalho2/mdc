using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volvo.Ecash.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/ecash/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public IActionResult Get()
        {
            return Ok("BE:1.005.0001");
        }
    }
}
