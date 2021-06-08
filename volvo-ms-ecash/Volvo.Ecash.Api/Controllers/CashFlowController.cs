using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/ecash/[controller]")]
    [ApiController]
    public class CashFlowController : APIController
    {
        private readonly ICashFlowService _service;
        private readonly IBankAccountService _bankAccountService;
        private readonly IUserService _userService;
        private readonly IDomainService _domainService;
        private readonly IExportService _exportService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="bankAccountService"></param>
        /// <param name="domainService"></param>
        public CashFlowController(ICashFlowService service,
            IBankAccountService bankAccountService,
            IUserService userService,
            IDomainService domainService,
            IExportService exportService)
        {
            _service = service;
            _bankAccountService = bankAccountService;
            _userService = userService;
            _domainService = domainService;
            _exportService = exportService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bankAccountId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("GetList")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetList([FromQuery] CashFlowFilters filters)
        {
            BankAccount ba = await _bankAccountService.GetByIdAsync(filters.BankAccountId);
            if (ba == null)
                return NotFound("Conta bancária não encontrada");

            return Ok(await _service.GetListCashFlowAsync(filters));
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
            var result = await _service.GetCashFlowByIdAsync(id);
            if (result == null)
                return NotFound();
            else
                _service.Delete(result);
            return NoContent();
        }

        [HttpPost("save")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Save([FromBody] List<CashFlow> inputModels)
        {
            if (inputModels.Count == 0)
                return BadRequest("Nada para salvar");
            var user = await _userService.SelectUserAsync(GetUserLogin()); //get user from header
            if (user == null)
                return BadRequest("Usuário não encontrado");


            try
            {
                inputModels.ForEach(t =>
                {
                    if (t.DomainId == 0)
                        throw new ArgumentException("Dominio não informado");
                    if (t.Domain != null)
                    {
                        if (t.Domain.InOut.Contains("IN"))
                            t.Amount = Math.Abs(t.Amount);
                        else
                            t.Amount = Math.Abs(t.Amount) * -1;
                    }
                    if (t.Amount > 99999999999999999999.99M || t.Amount < -99999999999999999999.99M)
                        throw new ArgumentException("Valor informado excede tamanho: 99999999999999999999");
                    t.Domain.IsDetailedTransaction ??= false;
                    if (t.Domain.IsDetailedTransaction.Value)
                    {
                        t.CashFlowDetaileds.ForEach(detail =>
                        {
                            if (t.Domain.InOut.Contains("IN"))
                                detail.Amount = Math.Abs(detail.Amount);
                            else
                                detail.Amount = Math.Abs(detail.Amount) * -1;
                        });
                    }
                });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            try
            {
                _service.Save(inputModels, user.UserID);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Não foi possivel salvar");
            }
        }

        [HttpPost("Undo")]
        [Authorize("Bearer")]
        public IActionResult Undo(DateTime date, int bankAccountId)
        {
            _service.Undo(date, bankAccountId);
            return Ok();
        }

        [HttpPost("UndoAll")]
        [Authorize("Bearer")]
        public IActionResult UndoAll(DateTime date, int bankAccountId)
        {
            _service.UndoAll(date, bankAccountId);
            return Ok();
        }


        [HttpPost("SaveAdjustment")]
        [Authorize("Bearer")]
        public async Task<IActionResult> SaveAdjustment([FromBody] Adjustment input)
        {
            if (input.Amount == 0)
                return BadRequest("Valor não pode ser zero");

            input.InOut = input.Amount > 0 ? "IN" : "OUT";
            var domainModel = await _domainService.GetAsync(new DomainFilters()
            {
                BankAccountId = input.BankAccountId,
                CategoryId = input.CategoryId,
                OperationId = input.OperationId,
                InOut = input.InOut
            });

            if (domainModel == null)
                return BadRequest("Domínio não encontrado");

            var user = await _userService.SelectUserAsync(GetUserLogin()); //get user from header
            if (user == null)
                return BadRequest("Usuário não encontrado");
            input.DomainId = domainModel.Id;
            try
            {
                return Ok(await _service.SaveAdjustment(input, user.UserID));
            }
            catch
            {
                return BadRequest("Não foi possível salvar");
            }
        }

        [HttpPost("SaveConciliation")]
        [Authorize("Bearer")]
        public async Task<IActionResult> SaveConciliation([FromBody] SaveConciliationModel model)
        {
            var user = await _userService.SelectUserAsync(GetUserLogin()); //get user from header
            if (user == null)
                return BadRequest("Usuário não encontrado");
            try
            {
                _service.SaveConciliation(DateTime.Now, model, user.UserID);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("TemplateReceivables")]
        [Authorize("Bearer")]
        public async Task<IActionResult> TemplateReceivables()
        {
            try
            {
                byte[] fileBytes = await _exportService.TemplateReceivables();
                var fileName = $"TemplateReceivables.xlsx";
                var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(fileBytes, mimeType, fileName);
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
        [HttpPost("UploadReceivables")]
        [Authorize("Bearer")]
        public async Task<IActionResult> UploadReceivables(IFormFile file, DateTime date)
        {
            var user = await _userService.SelectUserAsync(GetUserLogin()); //get user from header
            if (user == null)
                return BadRequest("Usuário não encontrado");

            await _service.UploadReceivables(file, date, user.UserID);
            return Ok();
        }
    }
}
