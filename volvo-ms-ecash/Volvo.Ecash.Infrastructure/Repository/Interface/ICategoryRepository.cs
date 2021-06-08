using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface ICategoryRepository {
        public Task<Category> InsertAsync (Category item);
        public Task<Category> UpdateAsync (Category item);
        public Task DeleteAsync (Category item);
        public Task<Category> GetAsync (int Id);
        public Task<List<Category>> GetListAsync();
        public Task InsertListAsync(List<Category> inputModel);
        public Task<List<Category>> GetListAsync(CategoryFilters filters);
    }
}
