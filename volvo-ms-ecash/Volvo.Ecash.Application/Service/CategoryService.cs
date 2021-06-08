using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public Task<Category> Create(Category inputModel)
        {
            return _repository.InsertAsync(inputModel);
        }

        public Task CreateList(List<Category> inputModel)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Category inputModel)
        {
            return _repository.DeleteAsync(inputModel);
        }

        public Task<List<Category>> GetListAsync()
        {
            return _repository.GetListAsync();
        }

        public Task<Category> GetByIdAsync(int id)
        {
            return _repository.GetAsync(id);
        }

        public Task Update(Category inputModel)
        {
            return _repository.UpdateAsync(inputModel);
        }

        public Task<List<Category>> GetListAsync(CategoryFilters filters)
        {
            return _repository.GetListAsync(filters);
        }
    }
}
