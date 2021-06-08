using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Volvo.Ecash.Dto.Model;

namespace Volvo.Ecash.Application.Service.Interface
{
    public interface IExportService
    {
        Task<byte[]> GenerateExcelFile(ExportCashFlowModel exportModel);
        Task<byte[]> GenerateKPIFile(KPIReport report);
        Task<byte[]> GenerateConciliationFile(ExportConciliationModel report);

        Task<byte[]> GenerateOperationalFile(ExportOperationalFilters filters);
        Task<byte[]> TemplateReceivables();
    }
}
