using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface IOperationService
    {
        Task<List<Operation>> GetListAsync();
        Task<Operation> GetByIdAsync(int id);
        Task<Operation> Create(Operation inputModel);
        Task Update(Operation inputModel);
        Task Delete(Operation inputModel);
        Task CreateList(List<Operation> inputModel);
        Task<List<Operation>> GetListAsync(OperationFilters filters);
    }
}
