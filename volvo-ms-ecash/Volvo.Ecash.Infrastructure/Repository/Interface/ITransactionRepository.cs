using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface ITransactionRepository
    {
        public Task<Transaction> InsertAsync(Transaction item);
        public Task<Transaction> UpdateAsync(Transaction item);
        public Task DeleteAsync(Transaction item);
        public Task<Transaction> SelectAsync(int Id);
        public Task<IEnumerable<Transaction>> GetListAsync();
        public Task InsertListAsync(List<Transaction> inputModel);
        public Task<List<Transaction>> GetListByAcctDateAsync(int bank_account_id, DateTime date);
        void UpdateConciliations(List<Transaction> transactions);
        Task<List<Transaction>> GetList(TransactionFilters filters);
        Task DeleteAsync(int bankAccountId, DateTime date);
        Task<DocumentUpload> InsertAsync(DocumentUpload documentUpload);
        Task<object> GetGroupedList(TransactionFilters filters);
    }
}
