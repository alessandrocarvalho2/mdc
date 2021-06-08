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
    public class CashFlowDetailedRepository : ICashFlowDetailedRepository
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public CashFlowDetailedRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Delete(CashFlowDetailed item)
        {
            CashFlowDetailed c = new CashFlowDetailed { Id = item.Id };
            _context.CashFlowTransactionsDetaileds.Attach(c).State = EntityState.Deleted;
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
        }

        public async Task DeleteAsync(CashFlowDetailed item)
        {
            _context.CashFlowTransactionsDetaileds.Remove(item);
            await _context.SaveChangesAsync();
            _context.Entry(item).State = EntityState.Detached;
        }

        public Task<CashFlowDetailed> GetAsync(int Id)
        {
            return _context.CashFlowTransactionsDetaileds.Where(e => e.Id == Id).FirstOrDefaultAsync();
        }

        public Task<List<CashFlowDetailed>> GetListAsync(int parentId)
        {
            return _context.CashFlowTransactionsDetaileds.Where(e => e.CashFlowId == parentId).ToListAsync();
        }

        public CashFlowDetailed Insert(CashFlowDetailed item)
        {
            _context.CashFlowTransactionsDetaileds.Attach(item).State = EntityState.Added;
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
            return item;
        }

        public Task<CashFlowDetailed> InsertAsync(CashFlowDetailed item)
        {
            _context.CashFlowTransactionsDetaileds.Add(item);
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
            return Task.FromResult(item);
        }

        public CashFlowDetailed Update(CashFlowDetailed item)
        {
            _context.CashFlowTransactionsDetaileds.Attach(item).State = EntityState.Modified;
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
            return item;
        }

        public Task<CashFlowDetailed> UpdateAsync(CashFlowDetailed item)
        {
            _context.CashFlowTransactionsDetaileds.Update(item);
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
            return Task.FromResult(item);
        }
    }
}
