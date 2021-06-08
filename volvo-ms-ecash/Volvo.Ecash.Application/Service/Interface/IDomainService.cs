using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface IDomainService
    {
        Task<List<DomainModel>> GetListAsync(DomainFilters filters);
        Task<DomainModel> GetByIdAsync(int id);
        Task<DomainModel> Create(DomainModel inputModel);
        Task Update(DomainModel inputModel);
        Task Delete(DomainModel inputModel);
        Task CreateList(List<DomainModel> inputModel);
        Task<DomainModel> GetAsync(DomainFilters filters);
        public Task<List<DomainModel>> GetListAsync(int bankAccountId);
    }
}
