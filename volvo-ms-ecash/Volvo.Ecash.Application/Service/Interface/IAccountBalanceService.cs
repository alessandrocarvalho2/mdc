using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface IAccountBalanceService
    {
        //Task<IEnumerable<AccountBalance>> GetListAsync();
        Task<AccountBalance> GetByIdAsync(int id);
        Task<AccountBalance> Create(AccountBalance inputModel);
        Task Update(AccountBalance inputModel);
        Task Delete(AccountBalance inputModel);
        Task<List<AccountBalance>> GetListByAccountAndDate(int BankAccountId, DateTime Date);
        Task<AccountBalance> Save(AccountBalance saveBody);
        Task<bool> IsAllowedToSave(DateTime date);
        public List<AccountBalance> GetListBetween(DateTime startDate, DateTime endDate);
        AccountBalance GetLastAcctBalance(BankAccount bankAccount);
    }
}
