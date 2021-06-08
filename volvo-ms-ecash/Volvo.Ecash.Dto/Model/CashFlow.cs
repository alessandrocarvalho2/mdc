using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    [Table("cash_flow_transactions")]
    public class CashFlow
    {
        public DomainModel Domain { get; set; }

        [Column("id")]
        public Int64 Id { get; set; }

        [Column("domain_id")]
        public int DomainId { get; set; }

        [Column("amount", TypeName = "numeric(30,2)")]
        public decimal Amount { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("approval")]
        public bool Approval { get; set; }

        [Column("create_at")]
        public DateTime CreateAt { get; set; }

        [Column("create_by")]
        public int CreateBy { get; set; }

        [Column("update_at")]
        public DateTime UpdateAt { get; set; }

        [Column("update_by")]
        public int UpdateBy { get; set; }

        [Column("manual_adjustment")]
        public bool ManualAdjustment { get; set; }

        //[Column("detailed_transaction")]
        //public bool? IsDetailedTransaction { get; set; }

        [Column("conciliation_id")]
        public Int64? ConciliationId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("is_distortion")]
        public bool? IsDistortion { get; set; }

        public List<CashFlowDetailed> CashFlowDetaileds { get; set; }
    }
}
