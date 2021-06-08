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
    public class CashFlowService : ICashFlowService
    {
        private readonly ICashFlowRepository _repository;
        private readonly IConciliationRepository _conciliationRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IAccountBalanceRepository _balanceRepository;
        private readonly IDomainRepository _domainRepository;
        private readonly IBankAccountService _bankAccountService;
        private readonly IHolidayService _holidayService;
        private readonly ExcelUtils _excelUtils;

        public CashFlowService(ICashFlowRepository repository,
            IConciliationRepository conciliationRepository,
            ITransactionRepository transactionRepository,
            IBankAccountRepository bankAccountRepository,
            IAccountBalanceRepository balanceRepository,
            IDomainRepository domainRepository,
            IHolidayService holidayService,
            IBankAccountService bankAccountService,
            ExcelUtils excelUtils
            )
        {
            _repository = repository;
            _conciliationRepository = conciliationRepository;
            _transactionRepository = transactionRepository;
            _bankAccountRepository = bankAccountRepository;
            _balanceRepository = balanceRepository;
            _domainRepository = domainRepository;
            _holidayService = holidayService;
            _bankAccountService = bankAccountService;
            _excelUtils = excelUtils;
        }

        public Task<List<CashFlow>> GetListCashFlowAsync(CashFlowFilters filters)
        {
            return _repository.GetListCashFlowAsync(filters);
        }

        public Task<CashFlow> GetCashFlowByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public void Save(List<CashFlow> inputModels, int userID)
        {
            inputModels.ForEach(t =>
            {
                //transação de SISCOMEX possui regra especial
                if (!t.Domain.IsDetailedTransaction.Value)
                {
                    NotDetailedCashFlow(t, userID);
                }
                else
                {
                    DetailedCashFlow(t, userID);
                }
            });
        }

        private void NotDetailedCashFlow(CashFlow t, int userId)
        {
            if (t.Amount != 0)
            {
                //Siscomex operations generate cash transfer based on calculations below
                if (t.Domain.Operation.Code.Equals("SIS"))
                {
                    //If result is less than zero, generates transfers in the system (means that needs cash)
                    decimal result = CalculateSiscomexTransfer(t);
                    if (result < 0)
                    {
                        GenerateTransfers(t, result, userId);
                    }
                }
                if (t.Id == 0)
                {
                    InsertCashFlow(t, userId);
                }
                else
                {
                    UpdateCashFlow(t, userId);
                }
            }
            else if (t.Id != 0)
            {
                Delete(t);
            }
        }

        private void GenerateTransfers(CashFlow flow, decimal t, int userId)
        {
            BankAccount mainAccount = _bankAccountRepository.GetMainAccount();
            if (mainAccount == null)
                throw new ArgumentException("Conta principal não encontrada");

            int destinyAccountId = _bankAccountRepository.Get(flow.Domain.BankAccountId).Id;

            DomainModel domainIn = _domainRepository.GetSiscomex(destinyAccountId, "IN");
            DomainModel domainOut = _domainRepository.GetSiscomex(mainAccount.Id, "OUT");

            //If SISCOMEX Transfer already exists, Update only...
            CashFlow sisIn = _repository.GetSiscomex(domainIn.Id, flow.Date);
            CashFlow sisOut = _repository.GetSiscomex(domainOut.Id, flow.Date);

            if (sisIn == null || sisOut == null)
            {
                //money leaves the account
                CashFlow outFlow = new CashFlow();
                if (domainOut == null)
                    throw new ArgumentException("Dominio de Siscomex da conta principal não encontrada");
                outFlow.DomainId = domainOut.Id;
                outFlow.Amount = t;
                outFlow.Date = flow.Date;
                outFlow.Approval = false;
                outFlow.CreateBy = userId;
                outFlow.CreateAt = DateTime.Now;

                //money comes to the account
                CashFlow inFlow = new CashFlow();
                if (domainIn == null)
                    throw new ArgumentException("Dominio de Siscomex da conta de destino não encontrada");
                inFlow.DomainId = domainIn.Id;
                inFlow.Amount = t * -1;
                inFlow.Date = flow.Date;
                inFlow.Approval = false;
                inFlow.CreateBy = userId;
                inFlow.CreateAt = DateTime.Now;

                InsertCashFlow(outFlow, userId);
                InsertCashFlow(inFlow, userId);
            }
            else
            {
                sisOut.Amount = t;
                sisOut.UpdateBy = userId;
                sisOut.UpdateAt = DateTime.Now;

                sisIn.Amount = t * -1;
                sisIn.UpdateBy = userId;
                sisIn.UpdateAt = DateTime.Now;

                UpdateCashFlow(sisIn, userId);
                UpdateCashFlow(sisOut, userId);
            }
        }

        private decimal CalculateSiscomexTransfer(CashFlow t)
        {
            DateTime utilDay = _holidayService.GetLastUtilDay(t.Date);
            BankAccount account = _bankAccountRepository.GetBalance(utilDay.Date, t.Domain.BankAccountId);
            if (account == null)
                throw new ArgumentException("Saldo não encontrado para realizar transação SISCOMEX");
            decimal actualBalance = account.AccountBalance.Balance;
            decimal minimumBalance = account.MinimumBalance;
            decimal result = actualBalance + t.Amount - minimumBalance;

            if (result < 0)
            {
                //Round thousand up
                decimal amountRound = result % 1000;
                result -= 1000 + amountRound;
            }

            return result;
        }

        private void DetailedCashFlow(CashFlow t, int userId)
        {
            if (t.CashFlowDetaileds != null)
            {
                t.CashFlowDetaileds.ForEach(d =>
                {
                    d.Delete ??= false;
                    d.Delete = d.Amount == Decimal.Zero ? true : d.Delete;
                    if (d.Delete.Value == true || d.Amount == decimal.Zero)
                    {
                        try
                        {
                            _repository.Delete(d);
                        }
                        catch { }
                    }
                });

                t.Amount = t.CashFlowDetaileds.Where(c => c.Delete.Value == false).Sum(c => c.Amount);
                if (t.Amount == Decimal.Zero && t.Id != 0)
                {
                    try
                    {
                        _repository.Delete(t);
                    }
                    catch { }
                }
                else
                {
                    if (t.Id == 0)
                    {
                        CashFlow resp = InsertCashFlow(t, userId);
                        t.Id = resp.Id;
                    }
                    else
                    {
                        UpdateCashFlow(t, userId);
                    }
                }
                t.CashFlowDetaileds.ForEach(d =>
                {
                    try
                    {
                        d.CashFlowId = t.Id;
                        if (d.Id == 0)
                        {
                            _repository.Insert(d);
                        }
                        else
                        {
                            _repository.Update(d);
                        }
                    }
                    catch { }

                });
            }
            else
            {
                _repository.Delete(t);
            }
        }

        public void Delete(CashFlow c)
        {
            _repository.Delete(c);
        }

        public CashFlow InsertCashFlow(CashFlow input, int userId)
        {
            input.CreateAt = DateTime.Now;
            input.CreateBy = userId;
            input.Domain = null;
            input.ManualAdjustment = false;
            return _repository.Insert(input);
        }

        public CashFlow UpdateCashFlow(CashFlow input, int userId)
        {
            input.UpdateAt = DateTime.Now;
            input.UpdateBy = userId;
            input.Domain = null;
            input.ManualAdjustment = false;
            return _repository.Update(input);
        }

        public void Undo(DateTime date, int bankAccountId)
        {
            _conciliationRepository.Undo(date, bankAccountId);
        }

        public void UndoAll(DateTime date, int bankAccountId)
        {
            _conciliationRepository.UndoAll(date, bankAccountId);
        }

        public Task<CashFlow> SaveAdjustment(Adjustment input, int userID)
        {
            CashFlow cf = new CashFlow()
            {
                Amount = input.Amount,
                Date = input.Date.Date,
                DomainId = input.DomainId,
                Approval = false,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                Domain = null,
                ManualAdjustment = true,
                Description = input.Description,
                IsDistortion = true
            };
            return Task.FromResult(_repository.Insert(cf));
        }

        public void SaveConciliation(DateTime now, SaveConciliationModel model, int userID)
        {
            if (model.CashFlows.Count == 0)
                throw new ArgumentException("Lista de conciliação vazia");

            Conciliation c = _conciliationRepository.Insert(now, model.Date.Date);

            model.CashFlows.ForEach(f => f.ConciliationId = c.Id);
            model.Transactions.ForEach(f => f.ConciliationId = c.Id);

            _repository.UpdateConciliations(model.CashFlows);
            if (model.Transactions.Count > 0)
            {
                _transactionRepository.UpdateConciliations(model.Transactions);
            }
        }

        public async Task<BankAccountDistortion> GetDistortionAsync(DateTime date, int bankAccountId)
        {
            CashFlowFilters f = new CashFlowFilters
            {
                BankAccountId = bankAccountId,
                Date = date,
                Distortion = true
            };

            return new BankAccountDistortion
            {
                BankAccountId = bankAccountId,
                Date = date,
                DistortionAmount = (await _repository.GetListCashFlowAsync(f)).Sum(e => e.Amount)
            };
        }

        public Task<bool> IsConciliated(DateTime date)
        {
            return _repository.IsConciliated(date);
        }

        public List<CashFlow> GetCashFlowsBetween(int bankAccountId, DateTime startDate, DateTime endDate)
        {
            return _repository.GetBetween(bankAccountId, startDate, endDate);
        }

        public Task UploadReceivables(IFormFile file, DateTime date, int userId)
        {
            string[] permittedExtensions = { ".xls", ".xlsx" };

            string ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                throw new ArgumentException("Extensão de arquivo não permitida");
            }
            if (file.Length <= 0)
            {
                throw new ArgumentException("Arquivo vazio");
            }
            List<DomainModel> domains = Task.Run(async () => await GetDomains()).Result;
            List<CashFlow> du = _excelUtils.ReadReceivables(file, domains);

            du.ForEach(c =>
            {
                c.CreateBy = userId;
                c.CreateAt = DateTime.Now;
                c.Date = date.Date;
                c.Approval = false;
                c.ManualAdjustment = false;
                _repository.Insert(c);
            });

            return Task.CompletedTask;
        }

        private async Task<List<DomainModel>> GetDomains()
        {
            BankAccount ba = _bankAccountRepository.GetMainAccount();
            DomainFilters filters = new DomainFilters();
            filters.InOut = "IN";
            filters.BankAccountId = ba.Id;
            filters.OperationId = 1; //Receivables
            filters.Visible = true;
            List<DomainModel> domainModel = await _domainRepository.GetListAsync(filters);
            return domainModel;
        }
    }
}
