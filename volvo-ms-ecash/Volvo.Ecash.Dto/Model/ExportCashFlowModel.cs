using System;
using System.Collections.Generic;
using System.Text;

namespace Volvo.Ecash.Dto.Model
{
    public class ExportCashFlowModel
    {
        public List<ExportCashFlowBank> ExportCashFlowBanks { get; set; }

        public DateTime Date { get; set; }
    }
}
