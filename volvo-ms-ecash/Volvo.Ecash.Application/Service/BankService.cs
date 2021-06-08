using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public class BankService : IService<Bank>
    {
        private readonly IRepository<Bank> _repository;

        public BankService(IRepository<Bank> repository)
        {
            _repository = repository;
        }

        public Task<Bank> Create(Bank inputModel)
        {
            return _repository.InsertAsync(inputModel);

        }

        public Task CreateList(List<Bank> inputModel)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Bank inputModel)
        {
            await _repository.DeleteAsync(inputModel);
        }

        public Task<List<Bank>> GetListAsync()
        {
            return _repository.GetListAsync();
        }

        public Task<Bank> GetByIdAsync(int id)
        {
            return _repository.GetAsync(id);
        }

        public Task Update(Bank inputModel)
        {
            return _repository.UpdateAsync(inputModel);
        }
    }
}
