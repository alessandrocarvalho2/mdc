using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface IBankAccountService
    {
        Task<BankAccount> GetByIdAsync(int id);
        Task<BankAccount> Create(BankAccount inputModel);
        Task Update(BankAccount inputModel);
        Task Delete(BankAccount inputModel);
        Task<List<BankAccount>> GetListAsync();

        public BankAccount Get(DateTime date, int bankAccountId);
        BankAccount GetMainAccount();
    }
}
