using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class BankAccountDistortion
    {
        public int BankAccountId { get; set; }
        public DateTime Date { get; set; }
        public decimal DistortionAmount { get; set; }
    }
}
