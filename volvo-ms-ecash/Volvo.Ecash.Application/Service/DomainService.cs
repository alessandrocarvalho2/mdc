using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public class DomainService : IDomainService
    {
        private readonly IDomainRepository _repository;

        public DomainService(IDomainRepository repository)
        {
            _repository = repository;
        }

        public Task<DomainModel> Create(DomainModel inputModel)
        {
            return _repository.InsertAsync(inputModel);
        }

        public Task CreateList(List<DomainModel> inputModel)
        {
            throw new NotImplementedException();
        }

        public Task Delete(DomainModel inputModel)
        {
            return _repository.DeleteAsync(inputModel);
        }

        public Task<List<DomainModel>> GetListAsync(DomainFilters filters)
        {
            return _repository.GetListAsync(filters);
        }

        public Task<DomainModel> GetByIdAsync(int id)
        {
            return _repository.GetAsync(id);
        }

        public Task Update(DomainModel inputModel)
        {
            return _repository.UpdateAsync(inputModel);
        }

        public Task<DomainModel> GetAsync(DomainFilters filters)
        {
            return _repository.GetAsync(filters);
        }

        public Task<List<DomainModel>> GetListAsync(int bankAccountId)
        {
            return _repository.GetListAsync(bankAccountId);
        }
    }
}
