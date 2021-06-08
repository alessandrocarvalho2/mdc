using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class SaveConciliationModel
    {
        public List<CashFlow> CashFlows { get; set; }
        public List<Transaction> Transactions { get; set; }
        public DateTime Date { get; set; }
    }
}
