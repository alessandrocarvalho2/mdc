using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface ICategoryService
    {
        Task<List<Category>> GetListAsync();
        Task<Category> GetByIdAsync(int id);
        Task<Category> Create(Category inputModel);
        Task Update(Category inputModel);
        Task Delete(Category inputModel);
        Task CreateList(List<Category> inputModel);
        Task<List<Category>> GetListAsync(CategoryFilters filters);
    }
}
