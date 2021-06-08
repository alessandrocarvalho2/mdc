using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Api.Controllers
{
    [Route("api/ecash/[controller]")]
    [ApiController]
    public class ReportController : APIController
    {
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly ICashFlowService _cashFlowService;
        private readonly ILogTransactionClosedService _logTransactionClosedService;
        private readonly IHolidayService _holidayService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="bankAccountService"></param>
        /// <param name="domainService"></param>
        public ReportController(IReportService reportService, 
            IUserService userService, 
            ICashFlowService cashFlowService,
            ILogTransactionClosedService logTransactionClosedService,
            IHolidayService holidayService)
        {
            _reportService = reportService;
            _userService = userService;
            _cashFlowService = cashFlowService;
            _logTransactionClosedService = logTransactionClosedService;
            _holidayService = holidayService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("GetCashConsolidationReport")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetCashConsolidationReport([FromQuery] DateTime date)
        {
            if (date == DateTime.MinValue)
                return BadRequest(string.Format(ErrorMessage.MSG003, "date"));

            DateTime dayBefore = _holidayService.GetLastUtilDay(date);
            CashConsolidationReport report = await _reportService.GetCashConsolidationReport(date, dayBefore);
            return Ok(report);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("GetCashTransferReport")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetCashTransferReport([FromQuery] DateTime date)
        {
            if (date == DateTime.MinValue)
                return BadRequest(string.Format(ErrorMessage.MSG003, "date"));
            DateTime dayBefore = _holidayService.GetLastUtilDay(date);

            try
            {
                List<CashTransferReport> report = await _reportService.GetListCashTransferReport(date, dayBefore);
                return Ok(report);
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
        [HttpPost("SaveCashTransferReport")]
        [Authorize("Bearer")]
        public async Task<IActionResult> SaveCashTransferReport([FromBody] DateTime date)
        {
            var user = await _userService.SelectUserAsync(GetUserLogin()); //get user from header
            if (user == null)
                return BadRequest("Usuário não encontrado");

            if (date == DateTime.MinValue)
                return BadRequest(string.Format(ErrorMessage.MSG003, "date"));

            DateTime dayBefore = _holidayService.GetLastUtilDay(date);

            List<CashFlow> result = await _reportService.GenerateCashTransferReport(date.Date, user.UserID, dayBefore);
            _cashFlowService.Save(result, user.UserID);
            await _logTransactionClosedService.Save(dayBefore, user.UserID);
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("GetTotalizationReport")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetTotalizationReport([FromQuery] CashFlowFilters filters)
        {
            if (filters.Date.Date == DateTime.MinValue)
                return BadRequest(string.Format(ErrorMessage.MSG003, "date"));
            try
            {
                return Ok(await _reportService.GetTotalizationReport(filters));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
