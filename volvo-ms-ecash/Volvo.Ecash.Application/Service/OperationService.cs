using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public class OperationService : IOperationService
    {
        private readonly IOperationRepository _repository;

        public OperationService(IOperationRepository repository)
        {
            _repository = repository;
        }

        public Task<Operation> Create(Operation inputModel)
        {
            return _repository.InsertAsync(inputModel);
        }

        public Task CreateList(List<Operation> inputModel)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Operation inputModel)
        {
            return _repository.DeleteAsync(inputModel);
        }

        public Task<List<Operation>> GetListAsync()
        {
            return _repository.GetListAsync();
        }

        public Task<Operation> GetByIdAsync(int id)
        {
            return _repository.GetAsync(id);
        }

        public Task Update(Operation inputModel)
        {
            return _repository.UpdateAsync(inputModel);
        }

        public Task<List<Operation>> GetListAsync(OperationFilters filters)
        {
            return _repository.GetListAsync(filters);
        }
    }
}
