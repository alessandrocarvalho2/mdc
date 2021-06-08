using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IRepository<T> where T : class {
        public Task<T> InsertAsync (T item);
        public Task<T> UpdateAsync (T item);
        public Task DeleteAsync (T item);
        public Task<T> GetAsync (int Id);
        public Task<List<T>> GetListAsync();
        public Task InsertListAsync(List<T> inputModel);
    }
}
