using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    [Table("category")]
    public class Category
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("export_import")]
        public bool ExportImport { get; set; }

        [Column("report_order")]
        public int? ReportOrder { get; set; }
    }
}
