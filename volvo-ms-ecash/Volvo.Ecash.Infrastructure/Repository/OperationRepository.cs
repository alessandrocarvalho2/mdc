using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Context;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class OperationRepository : IOperationRepository
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public OperationRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task DeleteAsync(Operation item)
        {
            _context.Operations.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Operation> InsertAsync(Operation item)
        {
            _context.Operations.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public Task InsertListAsync(List<Operation> inputModel)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Operation>> GetListAsync()
        {
            return await _context.Operations.ToListAsync();
        }

        public async Task<Operation> GetAsync(int Id)
        {
            return await _context.Operations.FirstOrDefaultAsync(e => Id == e.Id);
        }

        public async Task<Operation> UpdateAsync(Operation item)
        {
            _context.Operations.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public Task<List<Operation>> GetListAsync(OperationFilters filters)
        {
            var qDomains = _context.Domains.Where(d => d.BankAccountId == filters.BankAccountId);
            if (!string.IsNullOrEmpty(filters.InOut))
            {
                qDomains = qDomains.Where(d => d.InOut == filters.InOut);
            }
            if (filters.CategoryId.HasValue)
            {
                qDomains = qDomains.Where(d => d.CategoryId == filters.CategoryId.Value);
            }
            var query = from domains in qDomains
                        join ops in _context.Operations
                        on domains.OperationId equals ops.Id
                        select new Operation()
                        {
                            Id = ops.Id,
                            Description = ops.Description,
                            Code = ops.Code
                        };
            var distinct = query.Distinct();
            return distinct.ToListAsync();
        }
    }
}
