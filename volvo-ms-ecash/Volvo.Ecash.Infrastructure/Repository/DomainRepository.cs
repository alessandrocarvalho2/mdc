using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Infrastructure.Context;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;
using System.Linq;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class DomainRepository : IDomainRepository
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public DomainRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task DeleteAsync(DomainModel item)
        {
            _context.Domains.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<DomainModel> InsertAsync(DomainModel item)
        {
            _context.Domains.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public Task InsertListAsync(List<DomainModel> inputModel)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DomainModel>> GetListAsync(DomainFilters filters)
        {
            var resp = (from q in _context.Domains
                        join c in _context.Categories
                        on q.CategoryId equals c.Id
                        join o in _context.Operations
                        on q.OperationId equals o.Id
                        join ba in _context.BankAccounts
                        on q.BankAccountId equals ba.Id
                        select new DomainModel()
                        {
                            ApprovationNeeded = q.ApprovationNeeded,
                            BankAccountId = q.BankAccountId,
                            CategoryId = q.CategoryId,
                            Description = q.Description,
                            Id = q.Id,
                            InOut = q.InOut,
                            IsDetailedTransaction = q.IsDetailedTransaction,
                            OperationId = q.OperationId,
                            ReportOrder = q.ReportOrder,
                            Visible = q.Visible,
                            Operation = o,
                            Category = c,
                            BankAccount = ba
                        });
            if (filters.Id.HasValue)
            {
                resp = resp.Where(d => d.Id == filters.Id);
            }
            if (filters.BankAccountId.HasValue)
            {
                resp = resp.Where(d => d.BankAccountId == filters.BankAccountId);
            }
            if (filters.CategoryId.HasValue)
            {
                resp = resp.Where(d => d.CategoryId == filters.CategoryId);
            }
            if (filters.OperationId.HasValue)
            {
                resp = resp.Where(d => d.OperationId == filters.OperationId);
            }
            if (!string.IsNullOrEmpty(filters.InOut))
            {
                resp = resp.Where(d => d.InOut.Contains(filters.InOut));
            }
            if (!string.IsNullOrEmpty(filters.Description))
            {
                resp = resp.Where(d => d.Description.ToLower().Contains(filters.Description.ToLower()));
            }
            if (filters.AprovationNeeded.HasValue)
            {
                resp = resp.Where(d => d.ApprovationNeeded == filters.AprovationNeeded);
            }
            if (filters.Visible.HasValue)
            {
                resp = resp.Where(d => d.Visible == filters.Visible);
            }
            if (filters.DetailedTransaction.HasValue)
            {
                resp = resp.Where(d => d.IsDetailedTransaction == filters.DetailedTransaction);
            }

            return await resp.ToListAsync();
        }

        public async Task<DomainModel> GetAsync(int Id)
        {
            return await _context.Domains.FirstOrDefaultAsync(e => Id == e.Id);
        }

        public async Task<DomainModel> UpdateAsync(DomainModel item)
        {
            _context.Domains.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public DomainModel GetTransferDomainIn(CashTransferReport report)
        {
            DomainModel dm;
            dm = _context.Domains.Where(d =>
                            d.BankAccountId == report.DestinyAccount.Id
                            && d.Description.Contains(report.OriginAccount.Account)
                            && d.Description.Contains("Transf. entre contas")
                            && d.InOut.Contains("IN")
                            ).FirstOrDefault();
            if (dm == null)
            {
                throw new ArgumentException($"Domínio não encontrado para realizar a transação: IN => {report.DestinyAccount.Account} -> {report.OriginAccount.Account}");
            }
            dm.Operation = _context.Operations.FirstOrDefault(op => op.Id == dm.OperationId);
            dm.Category = _context.Categories.FirstOrDefault(ct => ct.Id == dm.CategoryId);
            return dm;
        }

        public DomainModel GetTransferDomainOut(CashTransferReport report)
        {
            DomainModel dm;
            dm = _context.Domains.Where(d =>
                            d.BankAccountId == report.OriginAccount.Id
                            && d.Description.Contains(report.DestinyAccount.Account)
                            && d.Description.Contains("Transf. entre contas")
                            && d.InOut.Contains("OUT")
                            ).FirstOrDefault();
            if (dm == null)
            {
                throw new ArgumentException($"Domínio não encontrado para realizar a transação: OUT => {report.OriginAccount.Account} -> {report.DestinyAccount.Account}");
            }
            dm.Operation = _context.Operations.FirstOrDefault(op => op.Id == dm.OperationId);
            dm.Category = _context.Categories.FirstOrDefault(ct => ct.Id == dm.CategoryId);
            return dm;
        }

        public Task<DomainModel> GetAsync(DomainFilters filters)
        {
            return _context.Domains.Where(e =>
                e.BankAccountId == filters.BankAccountId
                && e.CategoryId == filters.CategoryId
                && e.OperationId == filters.OperationId
                && e.InOut == filters.InOut).FirstOrDefaultAsync();
        }

        public DomainModel GetSiscomex(int bankAccountId, string inOut)
        {
            return _context.Domains.FirstOrDefault(e =>
            e.BankAccountId == bankAccountId
            && e.InOut == inOut
            && e.Description.ToLower().Contains("siscomex"));
        }

        public Task<List<DomainModel>> GetListAsync(int bankAccountId)
        {
            return _context.Domains.Where(e => e.BankAccountId == bankAccountId).ToListAsync();
        }
    }
}
