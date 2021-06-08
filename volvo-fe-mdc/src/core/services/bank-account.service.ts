import api from "./api";
import url from "../services/url.service";

export interface bankAccountModel {
  id: number;
  account: string;
  agency: string;
  nickname: string;
  isMainAccount: true;
  bankId: number;
  bank: {
    bankID: number;
    bankName: string;
    bankNickname: string;
  };
}

class BankAccountService {
  getAll(): any {
    return api
      .get(url.addr.get.BANK_ACCOUNT_LIST)
      .then((resp: any) => 
      resp.data);
  }
}

export default BankAccountService;
