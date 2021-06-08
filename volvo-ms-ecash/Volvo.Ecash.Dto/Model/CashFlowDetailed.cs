using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    [Table("cash_flow_transaction_detailed")]
    public class CashFlowDetailed
    {
        [Column("id")]
        public Int64 Id { get; set; }

        [Column("cash_flow_id")]
        public Int64 CashFlowId { get; set; }

        [Column("document_name")]
        public string DocumentName { get; set; }

        [Column("amount", TypeName = "decimal(30, 2)")]
        public decimal Amount { get; set; }

        [Column("detailed_description")]
        public string DetailedDescription { get; set; }

        [NotMapped]
        public bool? Delete { get; set; }

    }
}
