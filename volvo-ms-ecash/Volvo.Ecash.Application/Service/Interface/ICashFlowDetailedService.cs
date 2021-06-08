using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface ICashFlowDetailedService
    {
        Task<CashFlowDetailed> GetByIdAsync(int id);
        Task<CashFlowDetailed> Create(CashFlowDetailed inputModel);
        Task Update(CashFlowDetailed inputModel);
        Task Delete(CashFlowDetailed inputModel);

        public Task<List<CashFlowDetailed>> GetListFromParent(int parentId);
    }
}
