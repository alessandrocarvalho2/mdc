using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IReportRepository
    {
        Task<List<CashConsolidationItem>> GetListCCIAsync(DateTime date);
        decimal GetSumDbtCrdt(int bankAccountId, DateTime date);
    }
}
