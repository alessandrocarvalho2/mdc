using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Enum;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Context;
using Volvo.Ecash.Infrastructure.Repository.Interface;
using static Volvo.Ecash.Dto.Enum.EnumCommon;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class CashFlowRepository : ICashFlowRepository
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public CashFlowRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public void Delete(CashFlow input)
        {
            CashFlow c = new CashFlow { Id = input.Id };
            _context.CashFlowTransactions.Attach(c).State = EntityState.Deleted;
            _context.SaveChanges();
            _context.Entry(input).State = EntityState.Detached;
        }

        public Task<List<CashFlow>> GetListCashFlowAsync(CashFlowFilters filter)
        {
            var transactionsOfTheDay = _context.CashFlowTransactions.Where(t => t.Date.Date == filter.Date.Date);
            var domains = _context.Domains.Where(t => t.BankAccountId == filter.BankAccountId);
            var joins = from dm in domains
                        join cat in _context.Categories
                        on dm.CategoryId equals cat.Id
                        join op in _context.Operations
                        on dm.OperationId equals op.Id
                        select new DomainModel()
                        {
                            Operation = op,
                            Category = cat,
                            Id = dm.Id,
                            BankAccountId = dm.BankAccountId,
                            CategoryId = dm.CategoryId,
                            OperationId = dm.OperationId,
                            ApprovationNeeded = dm.ApprovationNeeded,
                            Description = dm.Description,
                            Visible = dm.Visible,
                            IsDetailedTransaction = dm.IsDetailedTransaction,
                            InOut = dm.InOut
                        };
            var query1 = from dm in joins
                         join cf in transactionsOfTheDay
                         on dm.Id equals cf.DomainId
                         into result
                         from cft in result.DefaultIfEmpty()
                         select new CashFlow()
                         {
                             Id = cft.Id,
                             DomainId = dm.Id,
                             Amount = cft.Amount,
                             Approval = cft.Approval,
                             Date = cft.Date,
                             CreateAt = cft.CreateAt,
                             CreateBy = cft.CreateBy,
                             UpdateAt = cft.UpdateAt,
                             UpdateBy = cft.UpdateBy,
                             ManualAdjustment = cft.ManualAdjustment,
                             ConciliationId = cft.ConciliationId,
                             Description = string.IsNullOrEmpty(cft.Description) ? dm.Description : cft.Description,
                             IsDistortion = cft.IsDistortion,
                             Domain = dm,
                         };

            if (filter.Id.HasValue)
            {
                query1 = query1.Where(e => filter.Id.Value == e.Id);
            }
            if (filter.CategoryId.HasValue)
            {
                query1 = query1.Where(e => filter.CategoryId.Value == e.Domain.CategoryId);
            }
            if (filter.OperationId.HasValue)
            {
                query1 = query1.Where(e => filter.OperationId.Value == e.Domain.OperationId);
            }
            if (!string.IsNullOrEmpty(filter.Description))
            {
                query1 = query1.Where(e => filter.Description == e.Domain.Description);
            }
            if (!string.IsNullOrEmpty(filter.InOut))
            {
                query1 = query1.Where(e => filter.InOut == e.Domain.InOut);
            }

            if (filter.Visible.HasValue)
            {
                if (filter.Visible.Value)
                {
                    query1 = query1 = query1.Where(e => e.Domain.Visible == true);
                }
                else
                {
                    query1 = query1 = query1.Where(e => e.Domain.Visible == false);
                }
            }

            if (filter.Approved.HasValue)
            {
                if (filter.Approved.Value == ApprovalCategory.AAprovar)
                {
                    query1 = query1 = query1.Where(e => e.Domain.ApprovationNeeded == true && e.Approval == false);
                }
                else if (filter.Approved.Value == ApprovalCategory.Aprovado)
                {
                    query1 = query1.Where(e => e.Domain.ApprovationNeeded == true && e.Approval == true);
                }
                else if (filter.Approved.Value == ApprovalCategory.PreAprovado)
                {
                    query1 = query1.Where(e => e.Domain.ApprovationNeeded == false);
                }
            }
            var lista = query1.ToList();
            if (filter.IncludeZeros.HasValue)
            {
                if (!filter.IncludeZeros.Value)
                {
                    lista = lista.Where(e => e.Amount != Decimal.Zero).ToList();
                }
            }

            lista.ForEach(c =>
            {
                if (c.Domain.IsDetailedTransaction ??= false)
                {
                    c.CashFlowDetaileds = _context.CashFlowTransactionsDetaileds.Where(d => d.CashFlowId == c.Id).ToList();
                }
            });

            if (filter.Conciliated.HasValue)
            {
                if (filter.Conciliated.Value)
                {
                    lista = lista.Where(e => e.ConciliationId != null).ToList();
                }
                else
                {
                    lista = lista.Where(e => e.ConciliationId == null).ToList();
                }
            }

            if (filter.Distortion.HasValue)
            {
                if (filter.Distortion.Value)
                {
                    lista = lista.Where(e => e.IsDistortion != null).ToList();
                }
                else
                {
                    lista = lista.Where(e => e.IsDistortion == null).ToList();
                }
            }

            return Task.FromResult(lista);
        }

        public void InsertCashFlow(List<CashFlow> inputs)
        {
            _context.CashFlowTransactions.AddRange(inputs);
            _context.SaveChanges();
            _context.Entry(inputs).State = EntityState.Detached;
        }

        public async Task<CashFlow> GetByIdAsync(Int64 id)
        {
            return await _context.CashFlowTransactions.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public void UpdateCashFlow(List<CashFlow> inputs)
        {
            inputs.ForEach(c =>
            {
                var dbEntry = _context.Entry(c);
                dbEntry.Property(x => x.UpdateAt).IsModified = true;
                dbEntry.Property(x => x.UpdateBy).IsModified = true;
                dbEntry.Property(x => x.Approval).IsModified = true;
                dbEntry.Property(x => x.Amount).IsModified = true;
            });
            _context.SaveChanges();
            _context.Entry(inputs).State = EntityState.Detached;
        }

        public CashFlow Insert(CashFlow input)
        {
            _context.CashFlowTransactions.Add(input);
            _context.SaveChanges();
            _context.Entry(input).State = EntityState.Detached;
            return input;
        }

        public CashFlow Update(CashFlow input)
        {
            var dbEntry = _context.Entry(input);
            dbEntry.Property(x => x.UpdateAt).IsModified = true;
            dbEntry.Property(x => x.UpdateBy).IsModified = true;
            dbEntry.Property(x => x.Approval).IsModified = true;
            dbEntry.Property(x => x.Amount).IsModified = true;
            _context.SaveChanges();
            _context.Entry(input).State = EntityState.Detached;
            return input;
        }

        public CashFlowDetailed Insert(CashFlowDetailed item)
        {
            _context.CashFlowTransactionsDetaileds.Attach(item).State = EntityState.Added;
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
            return item;
        }

        public CashFlowDetailed Update(CashFlowDetailed item)
        {
            _context.CashFlowTransactionsDetaileds.Attach(item).State = EntityState.Modified;
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
            return item;
        }

        public void Delete(CashFlowDetailed item)
        {
            CashFlowDetailed c = new CashFlowDetailed { Id = item.Id };
            _context.CashFlowTransactionsDetaileds.Attach(c).State = EntityState.Deleted;
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
        }

        public void UpdateConciliations(List<CashFlow> cashFlows)
        {
            cashFlows.ForEach(c =>
            {
                var dbEntry = _context.Entry(c);
                dbEntry.Property(x => x.ConciliationId).IsModified = true;
            });
            _context.SaveChanges();
        }

        public Task<bool> IsConciliated(DateTime date)
        {
            return _context.CashFlowTransactions.AnyAsync(c => c.Date.Date == date.Date && c.ConciliationId == null);
        }

        public List<CashFlow> GetBetween(int bankAccountId, DateTime startDate, DateTime endDate)
        {
            var domains = _context.Domains.Where(t => t.BankAccountId == bankAccountId);
            var joins = from dm in domains
                        join cat in _context.Categories
                        on dm.CategoryId equals cat.Id
                        join op in _context.Operations
                        on dm.OperationId equals op.Id
                        select new DomainModel()
                        {
                            Operation = op,
                            Category = cat,
                            Id = dm.Id,
                            BankAccountId = dm.BankAccountId,
                            CategoryId = dm.CategoryId,
                            OperationId = dm.OperationId,
                            ApprovationNeeded = dm.ApprovationNeeded,
                            Description = dm.Description,
                            Visible = dm.Visible,
                            IsDetailedTransaction = dm.IsDetailedTransaction,
                            InOut = dm.InOut
                        };
            var transactions = _context.CashFlowTransactions.Where(t => t.Date.Date >= startDate.Date && t.Date.Date <= endDate.Date);
            var query1 = from dm in joins
                         join cf in transactions
                         on dm.Id equals cf.DomainId
                         select new CashFlow()
                         {
                             Id = cf.Id,
                             DomainId = dm.Id,
                             Amount = cf.Amount,
                             Approval = cf.Approval,
                             Date = cf.Date,
                             CreateAt = cf.CreateAt,
                             CreateBy = cf.CreateBy,
                             UpdateAt = cf.UpdateAt,
                             UpdateBy = cf.UpdateBy,
                             ManualAdjustment = cf.ManualAdjustment,
                             ConciliationId = cf.ConciliationId,
                             Description = string.IsNullOrEmpty(cf.Description) ? dm.Description : cf.Description,
                             IsDistortion = cf.IsDistortion,
                             Domain = dm,
                         };
            return query1.ToList();
        }

        public CashFlow GetSiscomex(int domainId, DateTime date)
        {
            return _context.CashFlowTransactions.FirstOrDefault(e => e.DomainId == domainId && e.Date.Date == date.Date);
        }
    }
}
