using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volvo.Ecash.Dto.Model;
using Volvo.Ecash.Infrastructure.Context;
using Volvo.Ecash.Infrastructure.Repository.Interface;

namespace Volvo.Ecash.Infrastructure.Repository
{
    public class ConciliationRepository : IConciliationRepository
    {

        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public ConciliationRepository(BankContext bankContext, IMapper mapper)
        {
            _context = bankContext;
            _mapper = mapper;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public Conciliation Insert(DateTime now, DateTime date)
        {
            Conciliation c = new Conciliation { DateTime = now, Date = date.Date };
            _context.Conciliations.Add(c);
            _context.SaveChanges();
            _context.Entry(c).State = EntityState.Detached;
            return c;
        }


        public void Undo(DateTime date, int bankAccountId)
        {
            var resp = _context.Conciliations
                .Where(c => c.Date.Date == date.Date)
                .OrderByDescending(c => c.DateTime)
                .Select(c => c.Id)
                .FirstOrDefault();

            var domains = _context.Domains.Where(t => t.BankAccountId == bankAccountId);
            var transactions = _context.CashFlowTransactions.Where(t => t.ConciliationId == resp);

            var query1 = from dm in domains
                         join cf in transactions
                         on dm.Id equals cf.DomainId
                         select new CashFlow()
                         {
                             Id = cf.Id,
                             DomainId = dm.Id,
                             Amount = cf.Amount,
                             Approval = cf.Approval,
                             Date = cf.Date,
                             CreateAt = cf.CreateAt,
                             CreateBy = cf.CreateBy,
                             UpdateAt = cf.UpdateAt,
                             UpdateBy = cf.UpdateBy,
                             ManualAdjustment = cf.ManualAdjustment,
                             ConciliationId = cf.ConciliationId,
                             Description = cf.Description,
                             IsDistortion = cf.IsDistortion,
                             Domain = dm,
                         };

            var list = query1.ToList();


            list.ForEach(c => c.ConciliationId = null);
            _context.CashFlowTransactions.UpdateRange(list);

            var list2 = _context.Transactions.Where(t => t.ConciliationId == resp && t.BankAccountId == bankAccountId).ToList();
            list2.ForEach(c => c.ConciliationId = null);


            _context.Transactions.UpdateRange(list2);

            //Delete Adjustments
            var toDelete = list.Where(c => c.ManualAdjustment == true).ToList();
            toDelete.ForEach(c => _context.CashFlowTransactions.Remove(c));

            _context.SaveChanges();
        }

        public void UndoAll(DateTime date, int bankAccountId)
        {
            var resp = _context.Conciliations
                .Where(c => c.Date.Date == date.Date)
                .Select(c => c.Id)
                .ToList();



            resp.ForEach(id =>
            {
                var domains = _context.Domains.Where(t => t.BankAccountId == bankAccountId);
                var transactions = _context.CashFlowTransactions.Where(t => t.ConciliationId == id);

                var query1 = from dm in domains
                             join cf in transactions
                             on dm.Id equals cf.DomainId
                             select new CashFlow()
                             {
                                 Id = cf.Id,
                                 DomainId = dm.Id,
                                 Amount = cf.Amount,
                                 Approval = cf.Approval,
                                 Date = cf.Date,
                                 CreateAt = cf.CreateAt,
                                 CreateBy = cf.CreateBy,
                                 UpdateAt = cf.UpdateAt,
                                 UpdateBy = cf.UpdateBy,
                                 ManualAdjustment = cf.ManualAdjustment,
                                 ConciliationId = cf.ConciliationId,
                                 Description = cf.Description,
                                 IsDistortion = cf.IsDistortion,
                                 Domain = dm,
                             };

                var list = query1.ToList();

                list.ForEach(c => c.ConciliationId = null);
                _context.CashFlowTransactions.UpdateRange(list);

                var list2 = _context.Transactions.Where(t => t.ConciliationId == id && t.BankAccountId == bankAccountId).ToList();
                list2.ForEach(c => c.ConciliationId = null);
                _context.Transactions.UpdateRange(list2);

                //Delete Adjustments
                var toDelete = list.Where(c => c.ManualAdjustment == true).ToList();
                toDelete.ForEach(c => _context.CashFlowTransactions.Remove(c));
            });

            _context.SaveChanges();
        }

    }
}
