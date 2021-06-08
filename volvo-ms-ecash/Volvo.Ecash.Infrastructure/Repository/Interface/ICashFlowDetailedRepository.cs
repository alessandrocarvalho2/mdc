using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface ICashFlowDetailedRepository
    {
        public Task<CashFlowDetailed> InsertAsync(CashFlowDetailed item);
        public Task<CashFlowDetailed> UpdateAsync(CashFlowDetailed item);
        public Task DeleteAsync(CashFlowDetailed item);

        public CashFlowDetailed Insert(CashFlowDetailed item);
        public CashFlowDetailed Update(CashFlowDetailed item);
        public void Delete(CashFlowDetailed item);

        public Task<CashFlowDetailed> GetAsync(int Id);

        public Task<List<CashFlowDetailed>> GetListAsync(int parentId);
    }
}
