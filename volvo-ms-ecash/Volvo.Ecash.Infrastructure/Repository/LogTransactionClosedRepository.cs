using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Context;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class LogTransactionClosedRepository : ILogTransactionClosedRepository
    {
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public LogTransactionClosedRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public Task<bool> ExistsAsync(DateTime date)
        {
            return _context.LogsTransactionClosed.AnyAsync(e => e.Date.Date == date.Date);
        }

        public async Task Save(LogTransactionClosed log)
        {
            await _context.LogsTransactionClosed.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
