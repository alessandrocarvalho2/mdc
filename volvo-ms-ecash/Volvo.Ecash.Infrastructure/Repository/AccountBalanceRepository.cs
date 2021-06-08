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
    public class AccountBalanceRepository : IAccountBalanceRepository
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public AccountBalanceRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task DeleteAsync(AccountBalance item)
        {
            _context.AccountBalances.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<AccountBalance> InsertAsync(AccountBalance item)
        {
            _context.AccountBalances.RemoveRange(_context.AccountBalances.Where(ab => ab.BankAccountId == item.BankAccountId && ab.Date == item.Date));
            _context.AccountBalances.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public Task InsertListAsync(List<AccountBalance> inputModel)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AccountBalance>> GetListAsync()
        {
            return await _context.AccountBalances.ToListAsync();
        }

        public async Task<AccountBalance> GetAsync(int Id)
        {
            return await _context.AccountBalances.FirstOrDefaultAsync(e => Id == e.Id);
        }

        public async Task<AccountBalance> UpdateAsync(AccountBalance item)
        {
            _context.AccountBalances.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<AccountBalance> GetBy(DateTime date, int bankAccountId)
        {
            return await _context.AccountBalances.FirstOrDefaultAsync(e => e.BankAccountId == bankAccountId && e.Date == date);
        }

        public async Task<List<AccountBalance>> GetListByAcctDateAsync(int bankAccountId, DateTime date)
        {
            return await _context.AccountBalances.Where(e => e.BankAccountId == bankAccountId && e.Date == date).ToListAsync();
        }

        public AccountBalance Get(DateTime date, int bankAccountId)
        {
            return _context.AccountBalances.Where(e => e.BankAccountId == bankAccountId && e.Date == date).FirstOrDefault();
        }

        public List<AccountBalance> GetListBetween(DateTime startDate, DateTime endDate)
        {
            return _context.AccountBalances.Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date).ToList();
        }

        public AccountBalance GetLastAcctBalance(BankAccount bankAccount)
        {
            return _context.AccountBalances
                .Where(x => bankAccount.Id == x.BankAccountId)
                .OrderByDescending(x => x.Date)
                .FirstOrDefault();
        }
    }
}
