import api from "./api";
import url from "../services/url.service";
import qs from "qs";
import AccountBalanceModel from "../models/account-balance.model copy";

class AccountBalanceService {
  getList(bankAccountId: number, date: Date) {
    date = new Date(date.toDateString());
    const query = qs.stringify({
      date: date,
    });
    return api
      .get(
        url.addr.get.ACCOUNT_BALANCE_LIST + "/" + bankAccountId + `?${query}`
      )
      .then((resp: any) => resp.data);
  }

  isAllowedToSave(date: Date) {
    date = new Date(date.toDateString());
    const query = qs.stringify({
      date: date,
    });
    return api
      .get(url.addr.get.BANK_STATEMENT_ALLOW_SAVE + `/?${query}`)
      .then((resp: any) => resp.data);
  }

  save(accountBalance: AccountBalanceModel) {
    // date = new Date(date.toDateString());
    // const query = qs.stringify({
    //   bankAccountId: bankAccountId,
    //   date: date,
    //   amount: amount,
    // });

    return api
      .post(url.addr.post.ACCOUNT_BALANCE_SAVE, accountBalance)
      .then((resp: any) => resp.data);
  }
}
export default AccountBalanceService;
