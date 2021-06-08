import api from "./api";
import url from "../services/url.service";
import qs from "qs";
import DonwloadUploadModel from "../models/document-upload,model";
import TransactionFilterModel from "../models/filters/transaction-filter.model";

export interface bankAccount {
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

class BankStatementService {
  getAll() {
    return api
      .get(url.addr.get.BANK_STATEMENT_LIST)
      .then((resp: any) => resp.data);
  }

  getByAccount(id: number, date: Date) {
    date = new Date(date.toDateString());
    const query = qs.stringify({
      date: date,
    });

    return api
      .get(url.addr.get.BANK_STATEMENT_ACCOUNT + "/" + id + `?${query}`)
      .then((resp: any) => resp.data);
  }

  getList(filter: TransactionFilterModel) {
    if (filter?.date) filter.date = new Date(filter?.date.toDateString());
    const query = qs.stringify({
      bankAccountId: filter.bankAccountId,
      date: filter.date,
      conciliated: filter.conciliated,
    });

    return api
      .get(url.addr.get.BANK_STATEMENT_LIST  + `?${query}`)
      .then((resp: any) => resp.data);
  }

  addFile(id: number, file: any) {
    const query = qs.stringify({
      bank_account_id: id,
    });

    return api
      .post(url.addr.post.BANK_STATEMENT_UPLOAD + `?${query}`, file, {
        headers: { "Content-type": "multipart/form-data" },
      })
      .then((resp: any) => resp.data);
  }

  insert(donwloadUploadModel: DonwloadUploadModel) {
    return api
      .post(url.addr.post.BANK_STATEMENT_INSERT, donwloadUploadModel)
      .then((resp: any) => resp.data);
  }

  deleteByBankAndDate(id: number, date: Date) {
    date = new Date(date.toDateString());
    const query = qs.stringify({
      bankAccountId: id,
      date: date,
    });

    return api
      .delete(url.addr.delete.BANK_STATEMENT_DELETE + `?${query}`)
      .then((resp: any) => resp.data);
  }
}

export default BankStatementService;
