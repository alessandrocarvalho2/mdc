using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    [Table("transaction")]
    public class Transaction
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("bank_account_id")]
        public int BankAccountId { get; set; }

        [Column("operation_id")]
        public int OperationId { get; set; }

        [Column("in_out")]
        public string InOut { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("amount", TypeName = "numeric(20,2)")]
        public decimal Amount { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("document_upload_id")]
        public int DocumentUploadId { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("domain_id")]
        public int DomainId { get; set; }

        [Column("conciliation_id")]
        public Int64? ConciliationId { get; set; }

        public BankAccount BankAccount { get; set; }

        [Column("row_number")]
        public int? RowNumber { get; set; }
    }
}
