using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volvo.Ecash.Dto.Enum;

namespace Volvo.Ecash.Dto.Model
{
    public class DomainFilters
    {
        public int? BankAccountId { get; set; }

        public int? OperationId { get; set; }

        public int? CategoryId { get; set; }


        public string InOut { get; set; }

        public int? Id { get; set; }

        public string Description { get; set; }

        public bool? AprovationNeeded { get; set; }

        public bool? Visible { get; set; }

        public bool? DetailedTransaction { get; set; }

    }
}
