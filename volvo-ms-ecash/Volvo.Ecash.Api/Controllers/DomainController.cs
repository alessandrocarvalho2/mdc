using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/ecash/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly IDomainService _service;

        /// <summary>
        /// Contructor
        /// </summary>        
        /// <param name="service"></param>        
        public DomainController(IDomainService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetList([FromQuery] DomainFilters filters)
        {
            return Ok(await _service.GetListAsync(filters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Get([FromQuery] DomainFilters filters)
        {
            return Ok(await _service.GetAsync(filters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Create([FromBody] DomainModel inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            await _service.Create(inputModel);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Update(int id, [FromBody] DomainModel inputModel)
        {
            if (inputModel == null || id != inputModel.Id)
                return BadRequest();

            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            else
            {
                inputModel.Id = result.Id;
                await _service.Update(inputModel);
                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            else
                await _service.Delete(result);
            return NoContent();
        }
    }
}
