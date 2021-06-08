import qs from "qs";
import CashConciliationModel from "../models/cash-conciliation.model";
import api from "./api";
import url from "./url.service";

class CashConciliationService {

  save(cashConsolidation: CashConciliationModel) {

    console.log(cashConsolidation)
    return api
      .post(url.addr.post.CASH_CONCiLIATION_SAVE, cashConsolidation)
      .then((resp: any) => resp.data);
  }

  undo(bankAccountId: number, date: Date) {
    const query = qs.stringify({
      bankAccountId: bankAccountId,
      date: date,
    });
    return api
      .post(url.addr.post.CASH_CONCiLIATION_UNDO + `?${query}`)
      .then((resp: any) => resp.data);
  }

  undoAll(bankAccountId: number, date: Date) {
    const query = qs.stringify({
      bankAccountId: bankAccountId,
      date: date,
    });
    return api
      .post(url.addr.post.CASH_CONCiLIATION_UNDO_ALL + `?${query}`)
      .then((resp: any) => resp.data);
  }
}
export default CashConciliationService;
