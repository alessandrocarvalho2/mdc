using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class TotalizationReport
    {
        public List<TotalizationReportItemIn> ItemsIn { get; set; }

        public List<TotalizationReportItemOut> ItemsOut { get; set; }

        public decimal SubtotalIn { get; set; }
        public decimal SubtotalApproved { get; set; }
        public decimal SubtotalNotApproved { get; set; }
        public decimal SubtotalPreApproved { get; set; }
        public decimal SubtotalOut { get; set; }
        public decimal Total { get; set; }
    }
}
