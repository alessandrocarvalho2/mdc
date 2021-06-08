using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/ecash/[controller]")]
    [ApiController]
    public class AccountBalanceController : ControllerBase
    {
        private readonly IAccountBalanceService _service;
        private readonly IHolidayService _holidayService;

        /// <summary>
        /// Contructor
        /// </summary>        
        /// <param name="service"></param>        
        public AccountBalanceController(IAccountBalanceService service, IHolidayService holidayService)
        {
            _service = service;
            _holidayService = holidayService;
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
        /// <param name="BankAccountId"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        [HttpGet("GetByAccount/{bankAccountId}", Name = "Get AccountBalance By Account & Date")]
        public async Task<IActionResult> GetListByAccountAndDate(int bankAccountId, [FromQuery] DateTime? date)
        {
            DateTime DateValue = ((DateTime)(date.HasValue ? date : DateTime.Now)).Date;
            return Ok(await _service.GetListByAccountAndDate(bankAccountId, DateValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Create([FromBody] AccountBalance inputModel)
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
        [HttpPut("Update/{id}")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountBalance inputModel)
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
        [HttpDelete("Delete/{id}")]
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankAccountId"></param>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Save([FromBody] AccountBalance saveBody)
        {
            if (saveBody.BankAccountId == 0)
                return BadRequest(ErrorMessage.MSG003.Replace("{0}", "bankAccountId"));
            if (saveBody.Date.Date.CompareTo(DateTime.Now.Date) < 0)
                return BadRequest("Não é possível adicionar data anterior a D0");
            var lastUtilDay = _holidayService.GetLastUtilDay(saveBody.Date.Date);
            saveBody.Date = lastUtilDay;
            try
            {
                return Ok(await _service.Save(saveBody));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("IsAllowedToSave")]
        [Authorize("Bearer")]
        public async Task<IActionResult> IsAllowedToSave([FromQuery] DateTime date)
        {
            if (date.Date.CompareTo(DateTime.Now.Date) < 0)
                return BadRequest("Não é possível adicionar data anterior a D0");
            //var lastUtilDay = _holidayService.GetLastUtilDay(date.Date);
            return Ok(await _service.IsAllowedToSave(date.Date));
        }
    }
}
