using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Api.Controllers
{
    [Route("api/ecash/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IExportService _exportService;
        private readonly IReportService _reportService;
        private readonly ICashFlowService _cashFlowService;
        private readonly IBankAccountService _bankAccountService;
        private readonly IAccountBalanceService _accountBalanceService;
        private readonly IHolidayService _holidayService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="bankAccountService"></param>
        /// <param name="domainService"></param>
        public ExportController(
            IExportService exportService,
            IReportService reportService,
            ICashFlowService cashFlowService,
            IBankAccountService bankAccountService,
            IAccountBalanceService accountBalanceService,
            IHolidayService holidayService)
        {
            _exportService = exportService;
            _reportService = reportService;
            _cashFlowService = cashFlowService;
            _bankAccountService = bankAccountService;
            _accountBalanceService = accountBalanceService;
            _holidayService = holidayService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="bank_account_id"></param>
        /// <returns></returns>
        [HttpGet("ExportCashFlow")]
        [Authorize("Bearer")]
        public async Task<IActionResult> ExportCashFlow([FromQuery] CashFlowFilters filters)
        {
            filters.IncludeZeros = false;
            List<BankAccount> accounts = await _bankAccountService.GetListAsync();

            ExportCashFlowModel exportModel = new ExportCashFlowModel
            {
                ExportCashFlowBanks = new List<ExportCashFlowBank>()
            };
            exportModel.Date = filters.Date;

            accounts.ForEach(async bank =>
            {
                filters.BankAccountId = bank.Id;
                ExportCashFlowBank exportBank = new ExportCashFlowBank
                {
                    BankAccount = bank,
                    CashFlows = await _cashFlowService.GetListCashFlowAsync(filters),
                    TotalizationReport = await _reportService.GetTotalizationReport(filters)
                };
                exportModel.ExportCashFlowBanks.Add(exportBank);
            });

            var fileName = $"CashFlow_{filters.Date:dd/MM/yyyy}.xlsx";
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = await _exportService.GenerateExcelFile(exportModel);
            return File(fileBytes, mimeType, fileName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="bank_account_id"></param>
        /// <returns></returns>
        [HttpGet("ExportKPI")]
        [Authorize("Bearer")]
        public async Task<IActionResult> ExportKPI([FromQuery] KPIFilters filters)
        {
            if (filters.EndDate < filters.StartDate)
                return BadRequest("Data final anterior a data inicial");

            List<BankAccount> accounts = await _bankAccountService.GetListAsync();
            List<AccountBalance> balances = _accountBalanceService.GetListBetween(filters.StartDate, filters.EndDate);

            KPIReport report = new KPIReport
            {
                StartDate = filters.StartDate,
                EndDate = filters.EndDate,
                Accounts = accounts,
                Balances = balances
            };

            var fileName = $"KPI_{filters.StartDate:dd/MM/yyyy}_{filters.EndDate:dd/MM/yyyy}.xlsx";
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = await _exportService.GenerateKPIFile(report);
            return File(fileBytes, mimeType, fileName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="bank_account_id"></param>
        /// <returns></returns>
        [HttpGet("ExportConciliation")]
        [Authorize("Bearer")]
        public async Task<IActionResult> ExportConciliation([FromQuery] ConciliationFilters filters)
        {
            DateTime d1 = _holidayService.GetLastUtilDay(filters.Date);

            bool IsConciliated = !(await _cashFlowService.IsConciliated(filters.Date.Date));
            if (!IsConciliated)
                return BadRequest("Existem itens não conciliados, por gentileza verificar");

            ExportConciliationModel report = new ExportConciliationModel
            {
                Date = filters.Date,
                D1 = d1
            };

            var fileName = $"Conciliation_{filters.Date:dd/MM/yyyy}.xlsx";
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = await _exportService.GenerateConciliationFile(report);
            return File(fileBytes, mimeType, fileName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="bank_account_id"></param>
        /// <returns></returns>
        [HttpGet("ExportOperationalCF")]
        [Authorize("Bearer")]
        public async Task<IActionResult> ExportOperationalCF([FromQuery] ExportOperationalFilters filters)
        {
            if (filters.EndDate < filters.StartDate)
                return BadRequest("Data final anterior a data inicial");

            if (filters.EndDate.Date - filters.StartDate.Date > TimeSpan.FromDays(31))
                return BadRequest("Não é possível extrair relatórios com mais de 1 mês");

            var fileName = $"OperationalCashFlow_{filters.StartDate:dd/MM/yyyy}_{filters.EndDate:dd/MM/yyyy}.xlsx";
            var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            byte[] fileBytes = await _exportService.GenerateOperationalFile(filters);
            return File(fileBytes, mimeType, fileName);
        }

    }
}
