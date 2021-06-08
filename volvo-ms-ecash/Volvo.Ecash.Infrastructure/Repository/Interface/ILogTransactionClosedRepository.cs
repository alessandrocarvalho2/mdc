using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface ILogTransactionClosedRepository
    {
        Task Save(LogTransactionClosed log);
        Task<bool> ExistsAsync(DateTime date);
    }
}
