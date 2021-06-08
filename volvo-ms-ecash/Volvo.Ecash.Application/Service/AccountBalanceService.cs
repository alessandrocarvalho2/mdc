using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public class AccountBalanceService : IAccountBalanceService
    {
        private readonly IAccountBalanceRepository _repository;
        private readonly ILogTransactionClosedRepository _logRepository;
        private readonly IBankAccountService _bankAccountService;

        public AccountBalanceService(IAccountBalanceRepository repository, ILogTransactionClosedRepository logRepository, IBankAccountService bankAccountService)
        {
            _repository = repository;
            _logRepository = logRepository;
            _bankAccountService = bankAccountService;
        }

        public Task<AccountBalance> Create(AccountBalance inputModel)
        {
            return _repository.InsertAsync(inputModel);
        }

        public Task CreateList(List<AccountBalance> inputModel)
        {
            throw new NotImplementedException();
        }

        public Task Delete(AccountBalance inputModel)
        {
            return _repository.DeleteAsync(inputModel);
        }

        public Task<AccountBalance> GetByIdAsync(int id)
        {
            return _repository.GetAsync(id);
        }

        public Task Update(AccountBalance inputModel)
        {
            return _repository.UpdateAsync(inputModel);
        }

        public Task<List<AccountBalance>> GetListByAccountAndDate(int BankAccountId, DateTime Date)
        {
            return _repository.GetListByAcctDateAsync(BankAccountId, Date);
        }

        public List<AccountBalance> GetListBetween(DateTime startDate, DateTime endDate)
        {
            List<BankAccount> ba = Task.Run(() => _bankAccountService.GetListAsync()).Result;
            ba.ForEach(b =>
            {
                b.AccountBalance = _repository.GetLastAcctBalance(b);
                if (b.AccountBalance.Date < endDate)
                {
                    b.AccountBalance.Date = endDate;
                    b.AccountBalance.Id = 0;
                    var resp = Task.Run(() => _repository.InsertAsync(b.AccountBalance)).Result;
                }
            });

            return _repository.GetListBetween(startDate, endDate);
        }

        public async Task<AccountBalance> Save(AccountBalance saveBody)
        {
            if (await IsAllowedToSave(saveBody.Date.Date))
            {
                AccountBalance ab = await _repository.GetBy(saveBody.Date.Date, saveBody.BankAccountId);
                if (ab == null)
                {
                    ab = new AccountBalance
                    {
                        Balance = saveBody.Balance,
                        Date = saveBody.Date,
                        BankAccountId = saveBody.BankAccountId,
                        IsManualAdjusment = true,
                        DocumentUploadId = 0
                    };
                    return await _repository.InsertAsync(ab);
                }
                else
                {
                    ab.Balance = saveBody.Balance;
                    ab.IsManualAdjusment = true;
                    return await _repository.UpdateAsync(ab);
                }
            }
            else
            {
                throw new ArgumentException("Não é permitido salvar na data informada - Transferências já realizadas");
            }
        }

        public async Task<bool> IsAllowedToSave(DateTime date)
        {
            return !(await _logRepository.ExistsAsync(date));
        }

        public AccountBalance GetLastAcctBalance(BankAccount bankAccount)
        {
            return _repository.GetLastAcctBalance(bankAccount);
        }
    }
}
