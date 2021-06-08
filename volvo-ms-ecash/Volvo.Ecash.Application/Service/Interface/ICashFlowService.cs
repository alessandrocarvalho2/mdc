using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface ICashFlowService
    {
        Task<List<CashFlow>> GetListCashFlowAsync(CashFlowFilters filters);
        Task<CashFlow> GetCashFlowByIdAsync(int id);
        void Delete(CashFlow input);
        void Save(List<CashFlow> inputModels, int userID);
        public void Undo(DateTime date, int bankAccountId);
        public void UndoAll(DateTime date, int bankAccountId);
        Task<CashFlow> SaveAdjustment(Adjustment input, int userID);
        void SaveConciliation(DateTime now, SaveConciliationModel model, int userID);
        Task<bool> IsConciliated(DateTime date);
        public Task<BankAccountDistortion> GetDistortionAsync(DateTime date, int bankAccountId);

        public List<CashFlow> GetCashFlowsBetween(int bankAccountId, DateTime startDate, DateTime endDate);
        Task UploadReceivables(IFormFile file, DateTime date, int userId);
    }
}
