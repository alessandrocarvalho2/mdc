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
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly IExportService _exportService;
        private readonly IHolidayService _holidayService;

        /// <summary>
        /// Contructor
        /// </summary>        
        /// <param name="service"></param>        
        public TransactionController(ITransactionService service, 
            IExportService exportService, 
            IHolidayService holidayService)
        {
            _service = service;
            _exportService = exportService;
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
        [HttpGet("GetList")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetList([FromQuery] TransactionFilters filters)
        {
            return Ok(await _service.GetList(filters));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BankAccountId"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        [HttpGet("GetGroupedList")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetGroupedList([FromQuery] TransactionFilters filters)
        {
            return Ok(await _service.GetGroupedList(filters));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Create([FromBody] Transaction inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            await _service.Create(inputModel);
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost("createList")]
        [Authorize("Bearer")]
        public async Task<IActionResult> CreateList([FromBody] List<Transaction> inputModel)
        {
            if (inputModel == null)
                return BadRequest();

            await _service.CreateList(inputModel);
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
        public async Task<IActionResult> Update(int id, [FromBody] Transaction inputModel)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="bank_account_id"></param>
        /// <returns></returns>
        [HttpPost("Upload")]
        [Authorize("Bearer")]
        public async Task<IActionResult> FileUpload(IFormFile file, int bank_account_id, DateTime? date)
        {
            DateTime dateOfTheDocument = date.HasValue ? date.Value.Date : DateTime.Now.Date;
            try
            {
                DateTime lastUtilDay = _holidayService.GetLastUtilDay(dateOfTheDocument);
                var du = await _service.OnPostUploadAsync(file, bank_account_id, lastUtilDay);
                DocumentUpload arquivoInserido = await _service.InsertAsync(du);
                return Ok(arquivoInserido);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankAccountId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpDelete("DeleteByBankAndDate")]
        [Authorize("Bearer")]
        public async Task<IActionResult> FileDeleteByDate(int bankAccountId, DateTime date)
        {
            await _service.DeleteAsync(bankAccountId, date.Date);
            return Ok($"Transações e Saldos da data {date.Date} excluídos com sucesso");
        }
    }
}
