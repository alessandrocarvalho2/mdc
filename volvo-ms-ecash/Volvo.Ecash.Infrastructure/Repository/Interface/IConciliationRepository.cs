using System;
using System.Collections.Generic;
using System.Text;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Infrastructure.Repository.Interface
{
    public interface IConciliationRepository
    {
        Conciliation Insert(DateTime now, DateTime date);
        public void Undo(DateTime date, int bankAccountId);
        public void UndoAll(DateTime date, int bankAccountId);
    }
}
