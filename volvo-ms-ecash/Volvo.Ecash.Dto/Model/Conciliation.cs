using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    [Table("conciliation")]
    public class Conciliation
    {
        [Column("id")]
        public Int64 Id { get; set; }

        [Column("datetime_creation")]
        public DateTime DateTime { get; set; }

        [Column("conciliation_date")]
        public DateTime Date { get; set; }
    }
}
