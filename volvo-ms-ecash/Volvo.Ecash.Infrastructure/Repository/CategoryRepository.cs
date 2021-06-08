using System;
using System.Collections.Generic;
using System.Text;
using Volvo.Ecash.Infrastructure.Repository.Interface;
using Volvo.Ecash.Dto.Model;
using System.Threading.Tasks;
using Volvo.Ecash.Infrastructure.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task DeleteAsync(Category item)
        {
            _context.Categories.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> InsertAsync(Category item)
        {
            _context.Categories.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public Task InsertListAsync(List<Category> inputModel)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetListAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetAsync(int Id)
        {
            return await _context.Categories.FirstOrDefaultAsync(e => Id == e.Id);
        }

        public async Task<Category> UpdateAsync(Category item)
        {
            _context.Categories.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public Task<List<Category>> GetListAsync(CategoryFilters filters)
        {
            var qDomains = _context.Domains.Where(d => d.BankAccountId == filters.BankAccountId);
            if (!string.IsNullOrEmpty(filters.InOut))
            {
                qDomains = qDomains.Where(d => d.InOut == filters.InOut);
            }
            if (filters.OperationId.HasValue)
            {
                qDomains = qDomains.Where(d => d.OperationId == filters.OperationId.Value);
            }
            var query = from domains in qDomains
                        join categories in _context.Categories
                        on domains.CategoryId equals categories.Id
                        select new Category()
                        {
                            Id = categories.Id,
                            Description = categories.Description
                        };
            var distinct = query.Distinct();
            return distinct.ToListAsync();
        }
    }
}
