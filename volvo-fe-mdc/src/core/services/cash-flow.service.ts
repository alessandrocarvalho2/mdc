import api from "./api";
import url from "./url.service";
import qs from "qs";
import CashFlowModel from "../models/cash-flow.model";
import CashAdjustmentModel from "../models/cash-adjustment.model";
import CashFlowFilterModel from "../models/filters/cash-flow-filter.model";
import FileHelper from "../utils/file-helper.util";

class CashFlowService {
  getList(filter: CashFlowFilterModel) {
    const query = qs.stringify({
      bankAccountId: filter.bankAccountId,
      date: filter.date,
      categoryId: filter.categoryId,
      operationId: filter.operationId,
      approved: filter.approved,
      inOut: filter.inOut,
      includeZeros: filter.includeZeros,
      conciliated: filter.conciliated,
      visible: filter.visible,
    })

    return api
      .get(url.addr.get.CASH_FLOW_LIST + `?${query}`)
      .then((resp: any) => resp.data);
  }

  getTotalization(filter: CashFlowFilterModel) {
    const query = qs.stringify({
      bankAccountId: filter.bankAccountId,
      date: filter.date,
      categoryId: filter.categoryId,
      operationId: filter.operationId,
      approved: filter.approved,
      inOut: filter.inOut,
      includeZeros: filter.includeZeros,
      conciliated: filter.conciliated,
    });

    return api
      .get(url.addr.get.CASH_FLOW_TOTALIZATION + `?${query}`)
      .then((resp: any) => resp.data);
  }

  save(cashFlowlist: CashFlowModel[]) {
    return api
      .post(url.addr.post.CASH_FLOW_SAVE, cashFlowlist)
      .then((resp: any) => resp.data);
  }

  saveAdjustment(cashAdjustment: CashAdjustmentModel) {
    return api
      .post(url.addr.post.CASH_FLOW_SAVE_ADJUSTMENT, cashAdjustment)
      .then((resp: any) => resp.data);
  }

  upload(file: any) {
    const query = qs.stringify({
      date: new Date(new Date().toDateString()),
    });

    return api
      .post(url.addr.post.CASH_FLOW_UPLOAD + `?${query}`, file, {
        headers: { "Content-type": "multipart/form-data" },
      })
      .then((resp: any) => resp.data);
  }

  getTemplate() {
    return api
      .get(url.addr.get.CASH_FLOW_TEMPLATE, {
        responseType: "arraybuffer",
      })
      .then((resp: any) => {
        FileHelper.download(
          resp.data,
          "application/excel",
          "MovimentacaoCaixa_Template" +
            new Date().toLocaleDateString() +
            ".xlsx"
        );
      });
  }
}
export default CashFlowService;
