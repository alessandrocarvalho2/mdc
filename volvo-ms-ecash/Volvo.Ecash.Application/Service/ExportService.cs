using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service
{
    public class ExportService : IExportService
    {
        private readonly ExcelUtils _excelUtils;
        private readonly IBankAccountService _bankAccountService;
        private readonly IReportService _reportService;
        private readonly IAccountBalanceService _accountBalanceService;
        private readonly ICashFlowService _cashFlowService;
        private readonly ICategoryService _categoryService;
        private readonly IDomainService _domainService;

        public ExportService(ExcelUtils excelUtils,
            IBankAccountService bankAccountService,
            IReportService reportService,
            IAccountBalanceService accountBalanceService,
            ICashFlowService cashFlowService,
            ICategoryService categoryService,
            IDomainService domainService)
        {
            _excelUtils = excelUtils;
            _bankAccountService = bankAccountService;
            _accountBalanceService = accountBalanceService;
            _reportService = reportService;
            _cashFlowService = cashFlowService;
            _categoryService = categoryService;
            _domainService = domainService;
        }

        public Task<byte[]> GenerateExcelFile(ExportCashFlowModel exportModel)
        {
            return Task.FromResult(_excelUtils.GenerateExportCashFlow(exportModel));
        }

        public Task<byte[]> GenerateKPIFile(KPIReport report)
        {
            return Task.FromResult(_excelUtils.GenerateExportKPI(report));
        }

        public async Task<byte[]> GenerateConciliationFile(ExportConciliationModel report)
        {
            report.Accounts = await _bankAccountService.GetListAsync();
            report.AccountBalances = _accountBalanceService.GetListBetween(report.Date, report.Date);
            report.CashReport = await _reportService.GetCashConsolidationReport(report.Date, report.D1);

            report.Distortions = new List<BankAccountDistortion>();

            report.Accounts.ForEach(async acc =>
            {
                report.Distortions.Add(await _cashFlowService.GetDistortionAsync(report.Date, acc.Id));
            });

            return _excelUtils.GenerateExportConciliation(report);
        }

        public async Task<byte[]> GenerateOperationalFile(ExportOperationalFilters filters)
        {
            List<BankAccount> accounts = await _bankAccountService.GetListAsync();

            List<ExportCashFlowBank> exports = new List<ExportCashFlowBank>();

            accounts.ForEach(acc =>
            {
                ExportCashFlowBank flowBank = new ExportCashFlowBank
                {
                    BankAccount = acc,
                    CashFlows =  _cashFlowService.GetCashFlowsBetween(acc.Id, filters.StartDate, filters.EndDate)
                };
                exports.Add(flowBank);
            });

            return _excelUtils.GenerateOperationalReport(exports);
        }

        public async Task<byte[]> TemplateReceivables()
        {
            List<Category> categories = await _categoryService.GetListAsync();
            BankAccount bankAccount = _bankAccountService.GetMainAccount();
            ReceivablesTemplate templateModel = new ReceivablesTemplate();
            templateModel.MainAccount = bankAccount;
            templateModel.Categories = categories.Where(c => c.ExportImport).ToList();
            if (templateModel.Categories.Count == 0)
                throw new ApplicationException("Nenhuma categoria de Exportação/Importação cadastrada");
            templateModel.Domains = (await _domainService.GetListAsync(bankAccount.Id)).Where(d => d.InOut.Contains("IN") && d.Visible == true).ToList();
            return _excelUtils.GenerateReceivablesTemplate(templateModel);
        }
    }
}
