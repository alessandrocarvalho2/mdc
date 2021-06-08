using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    [Table("document_upload")]
    public class DocumentUpload
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("filename")]
        public string Filename { get; set; }

        [Column("uploaded_at")]
        public DateTime UploadedAt { get; set; }

        [Column("selected_account")]
        public int AccountId { get; set; }

        public AccountBalance AccountBalance { get; set; }
        public List<Transaction> Transactions { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}
