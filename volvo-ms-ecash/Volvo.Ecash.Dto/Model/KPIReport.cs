using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class KPIReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<BankAccount> Accounts { get; set; }

        public List<AccountBalance> Balances { get; set; }
    }
}
