using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class ExportConciliationModel
    {
        public DateTime Date { get; set; }
        public DateTime D1 { get; set; }
        public List<BankAccount> Accounts { get; set; }

        public List<AccountBalance> AccountBalances { get; set; }

        public List<BankAccountDistortion> Distortions { get; set; }

        public decimal TotalCashFlow { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalDistortion { get; set; }
        public CashConsolidationReport CashReport { get; set; }
    }
}
