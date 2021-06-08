using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class BankController : ControllerBase
    {
        private readonly IService<Bank> _service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public BankController(IService<Bank> service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetAll")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetList()
        {
            return Ok(await _service.GetListAsync());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        [Authorize("Bearer")]
        public async Task<Bank> GetBank(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Create([FromBody] Bank inputModel)
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
        /// <param name="bankinput"></param>
        /// <returns></returns>
        [HttpPut("Update/{id}")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Update(int id, [FromBody] Bank bankinput)
        {
            if (bankinput == null || id != bankinput.bankID)
                return BadRequest();

            var bank = await GetBank(id);
            if (bank == null)
                return NotFound();
            else
            {
                bankinput.bankID = bank.bankID;
                await _service.Update(bankinput);
                return Ok();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Delete(int id)
        {
            var bank = await GetBank(id);
            if (bank == null)
                return NotFound();
            else
                await _service.Delete(bank);
            return NoContent();
        }
    }
}
