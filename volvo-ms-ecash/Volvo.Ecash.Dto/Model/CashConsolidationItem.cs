using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class CashConsolidationItem
    {
        public BankAccount BankAccount { get; set; }
        public decimal Credits { get; set; }
        public decimal Debits { get; set; }
        public decimal ReserveAmount { get; set; }
        public decimal MinimumBalance { get; set; }
    }
}
