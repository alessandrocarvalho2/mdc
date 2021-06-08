using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IAccountBalanceRepository
    {
        public Task<AccountBalance> InsertAsync(AccountBalance item);
        public Task<AccountBalance> UpdateAsync(AccountBalance item);
        public Task DeleteAsync(AccountBalance item);
        public Task<AccountBalance> GetAsync(int Id);
        public AccountBalance Get(DateTime date, int bankAccountId);
        public Task InsertListAsync(List<AccountBalance> inputModel);
        public Task<AccountBalance> GetBy(DateTime date, int bankAccountId);
        public Task<List<AccountBalance>> GetListByAcctDateAsync(int bankAccountId, DateTime date);
        List<AccountBalance> GetListBetween(DateTime startDate, DateTime endDate);

        public AccountBalance GetLastAcctBalance(BankAccount bankAccount);
    }
}
