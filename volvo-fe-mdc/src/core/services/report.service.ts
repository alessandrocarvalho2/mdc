import api from "./api";
import url from "./url.service";
import qs from "qs";

class ReportService {
  getCashConsolidationReport(date: Date) {
    date = new Date(date.toDateString());
    const query = qs.stringify({
      date: date,
    });

    return api
      .get(url.addr.get.CASH_CONSOLIDATION_REPORT_LIST + `/?${query}`)
      .then((resp: any) => resp.data);
  }

  getCashTransferReport(date: Date) {
    date = new Date(date.toDateString());
    const query = qs.stringify({
      date: date,
    });

    return api
      .get(url.addr.get.TRANSFER_BETWEEN_ACCOUNTS_LIST + `/?${query}`)
      .then((resp: any) => resp.data);
  }

  saveCashTransferReport(date: Date) {
    date = new Date(date.toDateString());

    return api
      .post(url.addr.post.TRANSFER_BETWEEN_ACCOUNTS_SAVE, date)
      .then((resp: any) => resp.data);
  }
}
export default ReportService;
