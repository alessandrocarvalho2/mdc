using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface IReportService
    {
        Task<CashConsolidationReport> GetCashConsolidationReport(DateTime date, DateTime dayBefore);
        Task<List<CashTransferReport>> GetListCashTransferReport(DateTime date, DateTime dayBefore);
        Task<List<CashFlow>> GenerateCashTransferReport(DateTime date, int userId, DateTime dayBefore);
        Task<TotalizationReport> GetTotalizationReport(CashFlowFilters filters);
    }
}
