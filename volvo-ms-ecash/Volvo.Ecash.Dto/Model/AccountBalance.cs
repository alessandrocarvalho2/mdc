using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    [Table("account_balance")]
    public class AccountBalance
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("bank_account_id")]
        public int BankAccountId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("balance", TypeName = "numeric(20,2)")]
        public decimal Balance { get; set; }

        [Column("document_upload_id")]
        public int DocumentUploadId { get; set; }

        [Column("manual_adjustment")]
        public bool IsManualAdjusment { get; set; }

    }
}
