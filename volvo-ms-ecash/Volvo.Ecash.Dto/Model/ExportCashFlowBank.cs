using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class ExportCashFlowBank
    {
        public BankAccount BankAccount { get; set; }

        public List<CashFlow> CashFlows { get; set; }

        public TotalizationReport TotalizationReport { get; set; }
    }
}
