using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface IHolidayService
    {
        Task<List<Holiday>> GetListAsync();
        Task<Holiday> GetByIdAsync(int id);
        Task<Holiday> Create(Holiday input);
        Task Update(Holiday input);
        Task Delete(Holiday input);

        DateTime GetNextUtilDay(DateTime from);
        DateTime GetLastUtilDay(DateTime from);
    }
}
