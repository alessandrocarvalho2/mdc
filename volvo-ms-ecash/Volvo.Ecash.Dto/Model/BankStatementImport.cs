using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class BankStatementImport
    {
        public AccountBalance AccountBalance { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
