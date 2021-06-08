using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class CashTransferReport
    {
        public BankAccount OriginAccount { get; set; }
        public BankAccount DestinyAccount { get; set; }
        public decimal AmountToTransfer { get; set; }
    }
}
