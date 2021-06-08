import AccountBalanceModel from "./account-balance.model copy";
import BankModel from "./bank.model";

export interface BankAccountModel {
  id: number;
  agency: string;
  account: string;
  nickname: string;
  bankId: number;
  bank: BankModel;
  accountBalance: AccountBalanceModel;
}


export default BankAccountModel;
