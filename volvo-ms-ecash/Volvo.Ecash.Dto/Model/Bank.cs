using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{

    [Table("bank")]
    public class Bank
    {
        [Column("bank_id")]
        public int bankID { get; set; }

        [Column("bank_name")]
        public string bankName { get; set; }

        [Column("bank_code")]
        public string bankCode { get; set; }

    }
}
