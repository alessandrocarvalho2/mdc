using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class CashConsolidationReport
    {
        public List<CashConsolidationItem> CashConsolidationItems { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalCredits { get; set; }
        public decimal TotalDebits { get; set; }
        public decimal TotalReserve { get; set; }
        public decimal TotalMinimum { get; set; }

        public string Message { get; set; }
        public decimal ApplicationAmount { get; set; }
    }
}
