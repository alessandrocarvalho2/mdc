using System;
using System.Collections.Generic;
using System.Text;
using static Volvo.Ecash.Dto.Enum.EnumCommon;

namespace Volvo.Ecash.Dto.Model
{
    public class TotalizationReportItemOut
    {
        public string CategoryName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal SubTotalApproved { get; set; }
        public decimal SubTotalNotApproved { get; set; }
        public decimal SubTotalPreApproved { get; set; }
    }

    public class TotalizationReportItemIn
    {
        public string CategoryName { get; set; }
        public decimal SubTotal { get; set; }
    }
}
