using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IBankAccountRepository
    {
        public Task<BankAccount> InsertAsync(BankAccount item);
        public Task<BankAccount> UpdateAsync(BankAccount item);
        public Task DeleteAsync(BankAccount item);
        public Task<BankAccount> GetAsync(int Id);
        public BankAccount Get(int id);
        public Task<List<BankAccount>> GetListAsync();
        public Task<List<BankAccount>> GetListBalanceAsync(DateTime date);
        public BankAccount GetBalance(DateTime date, int bankAccountId);
        public BankAccount GetMainAccount();
    }
}
