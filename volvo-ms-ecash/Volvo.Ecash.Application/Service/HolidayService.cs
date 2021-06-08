using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Application.Service.Interface;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Application.Service
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _repository;

        public HolidayService(IHolidayRepository repository)
        {
            _repository = repository;
        }


        public Task<Holiday> Create(Holiday inputModel)
        {
            return _repository.Insert(inputModel);
        }

        public Task Delete(Holiday inputModel)
        {
            return _repository.Delete(inputModel);
        }

        public Task<Holiday> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<List<Holiday>> GetListAsync()
        {
            return _repository.GetListAsync();
        }

        public Task Update(Holiday inputModel)
        {
            return _repository.Update(inputModel);
        }

        public DateTime GetNextUtilDay(DateTime from)
        {
            throw new NotImplementedException();
        }

        public DateTime GetLastUtilDay(DateTime from)
        {
            bool isHoliday = true;
            DateTime nextPossibleDate = from;
            while (isHoliday || nextPossibleDate.DayOfWeek == DayOfWeek.Saturday || nextPossibleDate.DayOfWeek == DayOfWeek.Sunday)
            {
                nextPossibleDate = nextPossibleDate.AddDays(-1);
                isHoliday = _repository.CheckForHoliday(nextPossibleDate);
            }
            return nextPossibleDate;
        }
    }
}