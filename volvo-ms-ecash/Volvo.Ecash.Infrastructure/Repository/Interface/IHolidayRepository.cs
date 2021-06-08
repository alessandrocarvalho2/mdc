using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IHolidayRepository
    {
        public Task<List<Holiday>> GetListAsync();
        public Task<Holiday> Insert(Holiday input);
        public Task<Holiday> Update(Holiday input);
        public Task Delete(Holiday input);
        public Task<Holiday> GetByIdAsync(int id);
        bool CheckForHoliday(DateTime nextPossibleDate);
    }
}
