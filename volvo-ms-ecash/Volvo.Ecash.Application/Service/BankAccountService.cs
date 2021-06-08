using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _repository;

        public BankAccountService(IBankAccountRepository repository)
        {
            _repository = repository;
        }

        public Task<BankAccount> Create(BankAccount inputModel)
        {
            return _repository.InsertAsync(inputModel);
        }

        public Task CreateList(List<BankAccount> inputModel)
        {
            throw new NotImplementedException();
        }

        public Task Delete(BankAccount inputModel)
        {
            return _repository.DeleteAsync(inputModel);
        }

        public Task<List<BankAccount>> GetListAsync()
        {
            return _repository.GetListAsync();
        }

        public Task<BankAccount> GetByIdAsync(int id)
        {
            return _repository.GetAsync(id);
        }

        public Task Update(BankAccount inputModel)
        {
            return _repository.UpdateAsync(inputModel);
        }

        public BankAccount Get(DateTime date, int bankAccountId)
        {
            BankAccount ba = _repository.GetBalance(date, bankAccountId);
            if (ba == null)
                throw new ArgumentException("Saldo não encontrado para a data informada");
            return ba;
        }

        public BankAccount GetMainAccount()
        {
            BankAccount ba = _repository.GetMainAccount();
            if (ba == null)
                throw new ArgumentException("Conta principal não encontrada");
            return ba;
        }
    }
}
