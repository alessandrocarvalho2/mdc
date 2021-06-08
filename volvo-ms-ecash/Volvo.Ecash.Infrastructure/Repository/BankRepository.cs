using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Context;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class BankRepository : IRepository<Bank>
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public BankRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task DeleteAsync(Bank item)
        {
            _context.Banks.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Bank> InsertAsync(Bank item)
        {
            _context.Banks.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public Task InsertListAsync(List<Bank> inputModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<Bank>> GetListAsync()
        {
            return await _context.Banks.ToListAsync();
        }

        public async Task<Bank> GetAsync(int Id)
        {
            return await _context.Banks.FirstOrDefaultAsync(e => Id == e.bankID);
        }

        public async Task<Bank> UpdateAsync(Bank item)
        {
            _context.Banks.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
