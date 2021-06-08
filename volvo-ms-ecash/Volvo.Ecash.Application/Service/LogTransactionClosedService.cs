using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public class LogTransactionClosedService : ILogTransactionClosedService
    {
        private readonly ILogTransactionClosedRepository _repository;

        public LogTransactionClosedService(ILogTransactionClosedRepository repository)
        {
            _repository = repository;
        }
        public Task Save(DateTime date, int userID)
        {
            LogTransactionClosed log = new LogTransactionClosed();
            log.Date = date.Date;
            log.ClosedAt = DateTime.Now;
            log.UserId = userID;
            return _repository.Save(log);
        }
    }
}
