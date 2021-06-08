using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class Adjustment
    {
        [Required]
        public int BankAccountId { get; set; }
        
        [Required]
        public int CategoryId { get; set; }
        
        [Required]
        public int OperationId { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        public decimal Amount { get; set; }

        public string InOut { get; set; }
       
        public int DomainId { get; set; }
        
        public string Description { get; set; }

    }
}
