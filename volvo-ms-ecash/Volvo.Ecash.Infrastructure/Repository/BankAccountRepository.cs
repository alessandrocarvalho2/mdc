using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Context;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public BankAccountRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task DeleteAsync(BankAccount item)
        {
            _context.BankAccounts.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<BankAccount> InsertAsync(BankAccount item)
        {
            _context.BankAccounts.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<BankAccount>> GetListAsync()
        {
            return await (from ba in _context.BankAccounts
                          join b in _context.Banks
                          on ba.BankId equals b.bankID
                          select new BankAccount()
                          {
                              Id = ba.Id,
                              Account = ba.Account,
                              Agency = ba.Agency,
                              Nickname = ba.Nickname,
                              MinimumBalance = ba.MinimumBalance,
                              KpiTarget = ba.KpiTarget,
                              IsMainAccount = ba.IsMainAccount,
                              ReportOrder = ba.ReportOrder,
                              KpiOrder = ba.KpiOrder,
                              BalanceTolerance = ba.BalanceTolerance,
                              AccountingDescription = ba.AccountingDescription,
                              BankId = ba.BankId,
                              Bank = b
                          }
                            ).ToListAsync();
        }

        public async Task<BankAccount> GetAsync(int Id)
        {
            return await (from ba in _context.BankAccounts
                          join b in _context.Banks
                          on ba.BankId equals b.bankID
                          select new BankAccount()
                          {
                              Id = ba.Id,
                              Account = ba.Account,
                              Agency = ba.Agency,
                              Nickname = ba.Nickname,
                              MinimumBalance = ba.MinimumBalance,
                              KpiTarget = ba.KpiTarget,
                              IsMainAccount = ba.IsMainAccount,
                              ReportOrder = ba.ReportOrder,
                              KpiOrder = ba.KpiOrder,
                              BalanceTolerance = ba.BalanceTolerance,
                              AccountingDescription = ba.AccountingDescription,
                              BankId = ba.BankId,
                              Bank = b
                          }
                            ).FirstOrDefaultAsync(e => Id == e.Id);
        }

        public async Task<BankAccount> UpdateAsync(BankAccount item)
        {
            _context.BankAccounts.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<List<BankAccount>> GetListBalanceAsync(DateTime date)
        {
            return await (from ba in _context.BankAccounts
                          join ab in _context.AccountBalances
                          on ba.Id equals ab.BankAccountId
                          select new BankAccount()
                          {
                              Id = ba.Id,
                              Account = ba.Account,
                              Agency = ba.Agency,
                              Nickname = ba.Nickname,
                              MinimumBalance = ba.MinimumBalance,
                              KpiTarget = ba.KpiTarget,
                              IsMainAccount = ba.IsMainAccount,
                              ReportOrder = ba.ReportOrder,
                              KpiOrder = ba.KpiOrder,
                              BalanceTolerance = ba.BalanceTolerance,
                              AccountingDescription = ba.AccountingDescription,
                              BankId = ba.BankId,
                              AccountBalance = ab
                          }
                ).ToListAsync();
        }

        public BankAccount GetBalance(DateTime date, int bankAccountId)
        {
            BankAccount account = (from ba in _context.BankAccounts
                                   join ab in _context.AccountBalances
                                   on ba.Id equals ab.BankAccountId
                                   join bank in _context.Banks
                                   on ba.BankId equals bank.bankID
                                   where (ba.Id == bankAccountId && ab.Date.Date == date.Date)
                                   select new BankAccount()
                                   {
                                       Id = ba.Id,
                                       Account = ba.Account,
                                       Agency = ba.Agency,
                                       Nickname = ba.Nickname,
                                       MinimumBalance = ba.MinimumBalance,
                                       KpiTarget = ba.KpiTarget,
                                       IsMainAccount = ba.IsMainAccount,
                                       ReportOrder = ba.ReportOrder,
                                       KpiOrder = ba.KpiOrder,
                                       BalanceTolerance = ba.BalanceTolerance,
                                       AccountingDescription = ba.AccountingDescription,
                                       BankId = ba.BankId,
                                       AccountBalance = ab,
                                       Bank = bank
                                   }).FirstOrDefault();
            return account;

        }

        public BankAccount GetMainAccount()
        {
            return _context.BankAccounts.FirstOrDefault(ac => ac.IsMainAccount);
        }

        public BankAccount Get(int id)
        {
            return (from ba in _context.BankAccounts
                    join b in _context.Banks
                    on ba.BankId equals b.bankID
                    select new BankAccount()
                    {
                        Id = ba.Id,
                        Account = ba.Account,
                        Agency = ba.Agency,
                        Nickname = ba.Nickname,
                        MinimumBalance = ba.MinimumBalance,
                        KpiTarget = ba.KpiTarget,
                        IsMainAccount = ba.IsMainAccount,
                        ReportOrder = ba.ReportOrder,
                        KpiOrder = ba.KpiOrder,
                        BalanceTolerance = ba.BalanceTolerance,
                        AccountingDescription = ba.AccountingDescription,
                        BankId = ba.BankId,
                        Bank = b
                    }
                ).FirstOrDefault(e => id == e.Id);
        }
    }
}
