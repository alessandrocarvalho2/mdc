using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IOperationRepository {
        public Task<Operation> InsertAsync (Operation item);
        public Task<Operation> UpdateAsync (Operation item);
        public Task DeleteAsync (Operation item);
        public Task<Operation> GetAsync (int Id);
        public Task<List<Operation>> GetListAsync();
        public Task InsertListAsync(List<Operation> inputModel);
        public Task<List<Operation>> GetListAsync(OperationFilters filters);
    }
}
