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
    public class ReportRepository : IReportRepository
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public ReportRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<List<CashConsolidationItem>> GetListCCIAsync(DateTime date)
        {
            var allBankAccounts = from acct in _context.BankAccounts
                                  join bank in _context.Banks
                                  on acct.BankId equals bank.bankID
                                  select new BankAccount()
                                  {
                                      Id = acct.Id,
                                      Account = acct.Account,
                                      Agency = acct.Agency,
                                      Nickname = acct.Nickname,
                                      MinimumBalance = acct.MinimumBalance,
                                      BalanceTolerance = acct.BalanceTolerance,
                                      KpiTarget = acct.KpiTarget,
                                      IsMainAccount = acct.IsMainAccount,
                                      BankId = acct.BankId,
                                      Bank = bank
                                  };
            var acctBalances = _context.AccountBalances.Where(ab => ab.Date.Date == date.Date);

            var x = await (from ba in allBankAccounts
                           join ab in acctBalances
                           on ba.Id equals ab.BankAccountId
                           into result
                           from c in result.DefaultIfEmpty()
                           select new BankAccount()
                           {
                               Id = ba.Id,
                               Account = ba.Account,
                               Agency = ba.Agency,
                               Nickname = ba.Nickname,
                               MinimumBalance = ba.MinimumBalance,
                               BalanceTolerance = ba.BalanceTolerance,
                               KpiTarget = ba.KpiTarget,
                               IsMainAccount = ba.IsMainAccount,
                               BankId = ba.BankId,
                               Bank = ba.Bank,
                               AccountBalance = c,
                           }
                ).ToListAsync();

            List<CashConsolidationItem> items = new List<CashConsolidationItem>();
            x.ForEach(ba =>
            {
                items.Add(new CashConsolidationItem()
                {
                    BankAccount = ba
                });
            });

            return items;
        }

        public decimal GetSumDbtCrdt(int bankAccountId, DateTime date)
        {
            var transactionsOfTheDay = _context.CashFlowTransactions.Where(t => t.Date.Date == date.Date);
            var domains = _context.Domains.Where(t => t.BankAccountId == bankAccountId);
            var joins = from dm in domains
                        select new DomainModel()
                        {
                            Id = dm.Id,
                            BankAccountId = dm.BankAccountId,
                            CategoryId = dm.CategoryId,
                            OperationId = dm.OperationId,
                            ApprovationNeeded = dm.ApprovationNeeded,
                            Description = dm.Description,
                            Visible = dm.Visible,
                            InOut = dm.InOut
                        };
            var query1 = from dm in joins
                         join cf in transactionsOfTheDay
                         on dm.Id equals cf.DomainId
                         select new CashFlow()
                         {
                             Id = cf.Id,
                             DomainId = cf.DomainId,
                             Amount = cf.Amount,
                             Approval = cf.Approval,
                             Date = cf.Date,
                             CreateAt = cf.CreateAt,
                             CreateBy = cf.CreateBy,
                             UpdateAt = cf.UpdateAt,
                             UpdateBy = cf.UpdateBy,
                             Domain = dm,
                             IsDistortion = cf.IsDistortion,
                             ConciliationId = cf.ConciliationId,
                             Description = cf.Description,
                             ManualAdjustment = cf.ManualAdjustment
                         };
            return query1.Sum(e => e.Amount);
        }
    }
}
