using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface IService<T> where T : class
    {
        Task<List<T>> GetListAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> Create(T inputModel);
        Task Update(T inputModel);
        Task Delete(T inputModel);
        Task CreateList(List<T> inputModel);
    }
}
