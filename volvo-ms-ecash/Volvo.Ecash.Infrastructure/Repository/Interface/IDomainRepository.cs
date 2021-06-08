using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IDomainRepository
    {
        public Task<DomainModel> InsertAsync(DomainModel item);
        public Task<DomainModel> UpdateAsync(DomainModel item);
        public Task DeleteAsync(DomainModel item);
        public Task<DomainModel> GetAsync(int Id);
        public Task<List<DomainModel>> GetListAsync(DomainFilters filters);
        public Task InsertListAsync(List<DomainModel> inputModel);
        public DomainModel GetTransferDomainIn(CashTransferReport report);
        public DomainModel GetTransferDomainOut(CashTransferReport report);
        public Task<DomainModel> GetAsync(DomainFilters filters);

        DomainModel GetSiscomex(int destinyAccountId, string v);
        Task<List<DomainModel>> GetListAsync(int bankAccountId);
    }
}
