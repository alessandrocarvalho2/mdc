using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class TransactionFilters
    {
        [Required]
        public int BankAccountId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public bool? Conciliated { get; set; }
    }
}
