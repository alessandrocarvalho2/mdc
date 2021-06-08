import api from "./api";
import url from "../services/url.service";
import ExportModel from "../models/export.model";
import CashFlowFilterModel from "../models/filters/cash-flow-filter.model";
import FileHelper from "../utils/file-helper.util";

class ExportService {
  getExportReportKpi(filter: ExportModel) {
    var dataI = filter.dateI?.toISOString().slice(0, 10);
    var dataF = filter.dateF?.toISOString().slice(0, 10);

    return api
      .get(
        url.addr.get.EXPORT_REPORT_KPI +
          `?StartDate=` +
          dataI +
          `&EndDate=` +
          dataF,
        {
          responseType: "arraybuffer",
        }
      )
      .then((resp: any) => {
        FileHelper.download(
          resp.data,
          "application/excel",
          "SaldosBancarios_" + dataI + "_" + dataF + ".xlsx"
        );
      });
  }

  getExportReportConciliation(filter: ExportModel) {
    var dataI = filter.dateI?.toISOString().slice(0, 10);

    return api
      .get(url.addr.get.EXPORT_CONCILIATION_KPI + `?Date=` + dataI, {
        responseType: "arraybuffer",
      })
      .then((resp: any) => {
        FileHelper.download(
          resp.data,
          "application/excel",
          "ResumoConciliacao_" + dataI + ".xlsx"
        );
      });
  }

  getExportReportOperacionalcf(filter: ExportModel) {
    var dataI = filter.dateI?.toISOString().slice(0, 10);
    var dataF = filter.dateF?.toISOString().slice(0, 10);

    return api
      .get(
        url.addr.get.EXPORT_REPORT_OPERATIONALCF +
          `?StartDate=` +
          dataI +
          `&EndDate=` +
          dataF,
        {
          responseType: "arraybuffer",
        }
      )
      .then((resp: any) => {
        FileHelper.download(
          resp.data,
          "application/excel",
          "CaixaRealizado_" + dataI + "_" + dataF + ".xlsx"
        );
      });
  }

  getExportReportBanckTransaction(filter: CashFlowFilterModel) {
    var BankAccountId = filter.bankAccountId?.toString();
    var dataF = filter.date?.toISOString().slice(0, 10);

    return api
      .get(
        url.addr.get.EXPORT_REPORT_BANK_TRANSACTION +
          `?BankAccountId=` +
          BankAccountId +
          `&Date=` +
          dataF,
          {
            responseType: "arraybuffer",
          }
      )
      .then((resp: any) => {
        FileHelper.download(
          resp.data,
          "application/excel",
          "MovimentacaoCaixa_" + dataF + ".xlsx"
        );
      });
  }
}
export default ExportService;
