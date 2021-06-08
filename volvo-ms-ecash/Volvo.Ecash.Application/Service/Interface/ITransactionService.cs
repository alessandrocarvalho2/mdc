using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetListAsync();
        Task<Transaction> GetByIdAsync(int id);
        Task<Transaction> Create(Transaction inputModel);
        Task Update(Transaction inputModel);
        Task Delete(Transaction inputModel);
        Task CreateList(List<Transaction> inputModel);
        Task<List<Transaction>> GetListByAccountAndDate(int BankAccountId, DateTime Date);
        Task<List<Transaction>> GetList(TransactionFilters filters);
        public Task<DocumentUpload> OnPostUploadAsync(IFormFile file, int bankId, DateTime lastUtilDay);
        public Task<DocumentUpload> InsertAsync(DocumentUpload document);
        public Task DeleteAsync(int bankAccountId, DateTime date);
        Task<object> GetGroupedList(TransactionFilters filters);
    }
}
