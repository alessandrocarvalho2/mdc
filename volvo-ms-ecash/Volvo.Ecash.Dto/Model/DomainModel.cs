using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    [Table("domain")]
    public class DomainModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("operation_id")]
        public int OperationId { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("bank_account_id")]
        public int BankAccountId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("aprovation_needed")]
        public bool ApprovationNeeded { get; set; }

        [Column("visible")]
        public bool Visible { get; set; }

        [Column("in_out")]
        public string InOut { get; set; }

        [Column("detailed_transaction")]
        public bool? IsDetailedTransaction { get; set; }

        [Column("report_order")]
        public int? ReportOrder { get; set; }

        public Operation Operation { get; set; }
        public Category Category { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}
