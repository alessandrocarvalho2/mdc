using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface ICashFlowRepository
    {
        public Task<List<CashFlow>> GetListCashFlowAsync(CashFlowFilters filters);
        CashFlow Insert(CashFlow input);
        void Delete(CashFlow input);
        Task<CashFlow> GetByIdAsync(Int64 id);
        CashFlow Update(CashFlow input);
        CashFlowDetailed Insert(CashFlowDetailed input);
        CashFlowDetailed Update(CashFlowDetailed input);
        void Delete(CashFlowDetailed input);
        void UpdateConciliations(List<CashFlow> cashFlows);
        Task<bool> IsConciliated(DateTime date);
        List<CashFlow> GetBetween(int bankAccountId, DateTime startDate, DateTime endDate);
        CashFlow GetSiscomex(int domainId, DateTime date);
    }
}
