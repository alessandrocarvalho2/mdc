using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volvo.Ecash.Dto.Enum;
using static Volvo.Ecash.Dto.Enum.EnumCommon;

namespace Volvo.Ecash.Dto.Model
{
    public class CashFlowFilters
    {
        [Required]
        public int BankAccountId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int? CategoryId { get; set; }

        public string InOut { get; set; }

        public int? OperationId { get; set; }

        public string Description { get; set; }

        public ApprovalCategory? Approved { get; set; }

        public bool? Visible { get; set; }

        public bool? IncludeZeros { get; set; }

        public bool? Conciliated { get; set; }

        public bool? Distortion { get; set; }

        public int? Id { get; set; }
    }
}
