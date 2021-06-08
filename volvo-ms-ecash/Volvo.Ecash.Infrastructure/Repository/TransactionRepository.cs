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
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public TransactionRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task DeleteAsync(Transaction item)
        {
            _context.Transactions.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Transaction> InsertAsync(Transaction item)
        {
            _context.Transactions.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Transaction>> GetListAsync()
        {
            return await (from tr in _context.Transactions
                          join ba in _context.BankAccounts
                          on tr.BankAccountId equals ba.Id
                          select new Transaction()
                          {
                              BankAccount = ba,
                              Id = tr.Id,
                              BankAccountId = tr.BankAccountId,
                              OperationId = tr.OperationId,
                              InOut = tr.InOut,
                              Description = tr.Description,
                              Amount = tr.Amount,
                              Date = tr.Date,
                              DocumentUploadId = tr.DocumentUploadId,
                              ConciliationId = tr.ConciliationId,
                          }
                            ).ToListAsync();
        }

        public async Task<Transaction> SelectAsync(int Id)
        {
            return await _context.Transactions.FirstOrDefaultAsync(e => Id == e.Id);
        }

        public async Task<Transaction> UpdateAsync(Transaction item)
        {
            _context.Transactions.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task InsertListAsync(List<Transaction> transactions)
        {
            _context.Transactions.AddRange(transactions);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetListByAcctDateAsync(int bank_account_id, DateTime date)
        {
            var query1 = _context.Transactions.Where(t => t.Date == date && t.BankAccountId == bank_account_id);

            var result = await (from tr in query1
                                join ba in _context.BankAccounts
                                on tr.BankAccountId equals ba.Id
                                select new Transaction()
                                {
                                    BankAccount = ba,
                                    Id = tr.Id,
                                    BankAccountId = tr.BankAccountId,
                                    OperationId = tr.OperationId,
                                    InOut = tr.InOut,
                                    Description = tr.Description,
                                    Amount = tr.Amount,
                                    Date = tr.Date,
                                    DocumentUploadId = tr.DocumentUploadId,
                                    ConciliationId = tr.ConciliationId,
                                    RowNumber = tr.RowNumber,
                                    CategoryId = tr.CategoryId,
                                    DomainId = tr.DomainId
                                }).OrderBy(t => t.RowNumber).ToListAsync();
            return result;
        }

        public void UpdateConciliations(List<Transaction> transactions)
        {
            transactions.ForEach(c =>
            {
                var dbEntry = _context.Entry(c);
                dbEntry.Property(x => x.ConciliationId).IsModified = true;
            });
            _context.SaveChanges();
        }

        public async Task<List<Transaction>> GetList(TransactionFilters filters)
        {
            var query1 = _context.Transactions.Where(t => t.Date == filters.Date.Date && t.BankAccountId == filters.BankAccountId);

            var result = (from tr in query1
                          join ba in _context.BankAccounts
                          on tr.BankAccountId equals ba.Id
                          select new Transaction()
                          {
                              BankAccount = ba,
                              Id = tr.Id,
                              BankAccountId = tr.BankAccountId,
                              OperationId = tr.OperationId,
                              InOut = tr.InOut,
                              Description = tr.Description,
                              Amount = tr.Amount,
                              Date = tr.Date,
                              DocumentUploadId = tr.DocumentUploadId,
                              ConciliationId = tr.ConciliationId,
                              CategoryId = tr.CategoryId,
                              DomainId = tr.DomainId,
                              RowNumber = tr.RowNumber
                          }).OrderBy(t => t.RowNumber);

            var list = await result.ToListAsync();

            if (filters.Conciliated.HasValue)
            {
                if (filters.Conciliated.Value)
                {
                    list = list.Where(e => e.ConciliationId != null).ToList();
                }
                else
                {
                    list = list.Where(e => e.ConciliationId == null).ToList();
                }
            }

            return await Task.FromResult(list);
        }

        public async Task DeleteAsync(int bankAccountId, DateTime date)
        {
            _context.Transactions.RemoveRange(_context.Transactions.Where(t => t.BankAccountId == bankAccountId && t.Date == date));
            _context.AccountBalances.RemoveRange(_context.AccountBalances.Where(ab => ab.BankAccountId == bankAccountId && ab.Date == date));
            await _context.SaveChangesAsync();
        }

        public async Task<DocumentUpload> InsertAsync(DocumentUpload item)
        {
            DocumentUploadDto dto = _mapper.Map<DocumentUploadDto>(item);
            await _context.DocumentUploads.AddAsync(dto);
            await _context.SaveChangesAsync();
            item.Transactions.ForEach(t =>
            {
                t.DocumentUploadId = dto.Id;
            });
            await _context.Transactions.AddRangeAsync(item.Transactions);
            if (item.AccountBalance != null
                && !_context.AccountBalances
                .Where(ab => ab.Date == item.AccountBalance.Date
                && ab.BankAccountId == item.AccountBalance.BankAccountId).Any())
            {
                item.AccountBalance.DocumentUploadId = dto.Id;
                item.AccountBalance.IsManualAdjusment = false;
                await _context.AccountBalances.AddAsync(item.AccountBalance);
            }
            await _context.SaveChangesAsync();
            item.Id = dto.Id;
            return item;
        }

        public async Task<object> GetGroupedList(TransactionFilters filters)
        {
            List<GroupedTransaction> groupedTransactions = new List<GroupedTransaction>();
            var query1 = _context.Transactions.Where(t => t.Date == filters.Date.Date && t.BankAccountId == filters.BankAccountId);

            var result = query1.ToList();

            if (filters.Conciliated.HasValue)
            {
                if (filters.Conciliated.Value)
                {
                    result = result.Where(e => e.ConciliationId != null).ToList();
                }
                else
                {
                    result = result.Where(e => e.ConciliationId == null).ToList();
                }
            }

            var grouping = result.GroupBy(x => x.Description);

            foreach (var group in grouping)
            {
                GroupedTransaction gp = new GroupedTransaction();
                gp.Description = group.Key;
                gp.Amount = group.Sum(g => g.Amount);
                gp.Transactions = new List<Transaction>();
                gp.Transactions.AddRange(group);

                groupedTransactions.Add(gp);
            }

            return await Task.FromResult(groupedTransactions);
        }
    }
}
