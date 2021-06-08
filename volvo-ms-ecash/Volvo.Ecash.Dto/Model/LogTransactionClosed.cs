using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    [Table("log_transaction_closed")]
    public class LogTransactionClosed
    {
        [Column("id")]
        public Int64 Id { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("closed_at")]
        public DateTime ClosedAt { get; set; }
    }
}
