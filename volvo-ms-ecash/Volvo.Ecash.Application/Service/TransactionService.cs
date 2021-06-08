using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Application.Utils;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly IBankAccountService _bankAccountService;
        private readonly IAccountBalanceService _accountBalanceService;
        private readonly ExcelUtils _utils;

        private readonly string[] permittedExtensions = { ".xls", ".xlsx" };

        public TransactionService(ITransactionRepository repository, 
            IBankAccountService bankAccountService,
            ExcelUtils excelUtils,
            IAccountBalanceService accountBalanceService)
        {
            _repository = repository;
            _bankAccountService = bankAccountService;
            _utils = excelUtils;
            _accountBalanceService = accountBalanceService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<DocumentUpload> OnPostUploadAsync(IFormFile file, int bankId, DateTime lastUtilDay)
        {
            string ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                throw new ArgumentException("Extensão de arquivo não permitida");
            }
            if (file.Length <= 0)
            {
                throw new ArgumentException("Arquivo vazio");
            }
            BankAccount bankAccount = await _bankAccountService.GetByIdAsync(bankId);
            if (bankAccount == null)
                throw new ArgumentException("Conta não encontrada");

            try
            {
                DocumentUpload du = _utils.ReadExcelFile(file, bankAccount, lastUtilDay);
                if (du.AccountBalance == null)
                {
                    du.AccountBalance = _accountBalanceService.GetLastAcctBalance(bankAccount);
                    du.AccountBalance.Date = lastUtilDay;
                    du.AccountBalance.Id = 0;
                }
                return du;
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Falha na leitura do arquivo: {file.FileName}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentUpload"></param>
        /// <returns></returns>
        public async Task<DocumentUpload> InsertAsync(DocumentUpload documentUpload)
        {
            return await _repository.InsertAsync(documentUpload);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int bankAccountId, DateTime date)
        {
            await _repository.DeleteAsync(bankAccountId, date);
        }

        public TransactionService(ITransactionRepository repository)
        {
            _repository = repository;
        }

        public Task<Transaction> Create(Transaction inputModel)
        {
            return _repository.InsertAsync(inputModel);
        }

        public async Task CreateList(List<Transaction> inputModel)
        {
            await _repository.InsertListAsync(inputModel);
        }

        public Task Delete(Transaction inputModel)
        {
            return _repository.DeleteAsync(inputModel);
        }

        public Task<IEnumerable<Transaction>> GetListAsync()
        {
            return _repository.GetListAsync();
        }

        public Task<List<Transaction>> GetListByAccountAndDate(int BankAccountId, DateTime Date)
        {
            return _repository.GetListByAcctDateAsync(BankAccountId, Date);
        }

        public Task<Transaction> GetByIdAsync(int id)
        {
            return _repository.SelectAsync(id);
        }

        public Task Update(Transaction inputModel)
        {
            return _repository.UpdateAsync(inputModel);
        }

        public Task<List<Transaction>> GetList(TransactionFilters filters)
        {
            return _repository.GetList(filters);
        }

        public Task<object> GetGroupedList(TransactionFilters filters)
        {
            return _repository.GetGroupedList(filters);
        }
    }
}
