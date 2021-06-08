using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public abstract class AbstractService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public AbstractService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public Task<T> Create(T inputModel)
        {
            throw new NotImplementedException();
        }

        public Task CreateList(List<T> inputModel)
        {
            throw new NotImplementedException();
        }

        public Task Delete(T inputModel)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> SelectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> SelectByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(T inputModel)
        {
            throw new NotImplementedException();
        }
    }
}
