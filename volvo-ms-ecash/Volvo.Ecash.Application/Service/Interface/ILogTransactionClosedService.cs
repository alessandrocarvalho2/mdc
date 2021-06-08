using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface ILogTransactionClosedService
    {
        Task Save(DateTime date, int userID);
    }
}
