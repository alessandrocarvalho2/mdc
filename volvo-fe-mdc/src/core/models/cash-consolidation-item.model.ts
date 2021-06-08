import BankAccountModel from "./bank-account.interface";

export interface CashConsolidationItemModel {
    id?: number;
    bankAccount: BankAccountModel;
    credits?: number;
    debits?: number;
    reserveAmount?: number;
    minimumBalance?: number;
 
  }

  export default CashConsolidationItemModel;
