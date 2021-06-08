using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Context;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly UtilsContext _context;
        private readonly IMapper _mapper;

        public HolidayRepository(UtilsContext utilsContext, IMapper mapper)
        {
            _context = utilsContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public bool CheckForHoliday(DateTime date)
        {
            return _context.Holidays.Any(h => h.Date == date);
        }

        public async Task Delete(Holiday input)
        {
            _context.Holidays.Remove(input);
            await _context.SaveChangesAsync();
        }

        public Task<Holiday> GetByIdAsync(int id)
        {
            return _context.Holidays.Where(h => h.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Holiday>> GetListAsync()
        {
            return await _context.Holidays.ToListAsync();
        }

        public async Task<Holiday> Insert(Holiday input)
        {
            await _context.Holidays.AddAsync(input);
            await _context.SaveChangesAsync();
            return input;
        }

        public async Task<Holiday> Update(Holiday input)
        {
            _context.Holidays.Update(input);
            await _context.SaveChangesAsync();
            return input;
        }
    }
}
