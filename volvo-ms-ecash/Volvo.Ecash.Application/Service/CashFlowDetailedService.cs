using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public class CashFlowDetailedService : ICashFlowDetailedService
    {
        private readonly ICashFlowDetailedRepository _cashFlowDetailedRepository;
        public CashFlowDetailedService(ICashFlowDetailedRepository cashFlowDetailedRepository)
        {
            _cashFlowDetailedRepository = cashFlowDetailedRepository;
        }

        public Task<CashFlowDetailed> Create(CashFlowDetailed input)
        {
            if (input.CashFlowId == 0)
                throw new ArgumentException("Necessário informar ID da transação pai");
            if (input.Amount == 0)
                throw new ArgumentException("Necessário informar valor da transação");

            return _cashFlowDetailedRepository.InsertAsync(input);
        }

        public Task Delete(CashFlowDetailed inputModel)
        {
            if (inputModel.Id == 0)
                throw new ArgumentException("Necessário informar Id da transação");
            return _cashFlowDetailedRepository.DeleteAsync(inputModel);
        }

        public Task<CashFlowDetailed> GetByIdAsync(int id)
        {
            if (id == 0)
                throw new ArgumentException("Necessário informar Id da transação");
            return _cashFlowDetailedRepository.GetAsync(id);
        }

        public Task Update(CashFlowDetailed inputModel)
        {
            if (inputModel.Id == 0)
                throw new ArgumentException("Necessário informar Id da transação");
            return _cashFlowDetailedRepository.UpdateAsync(inputModel);
        }

        public Task<List<CashFlowDetailed>> GetListFromParent(int parentId)
        {
            if (parentId == 0)
                throw new ArgumentException("Necessário informar Id");
            return _cashFlowDetailedRepository.GetListAsync(parentId);
        }
    }
}
