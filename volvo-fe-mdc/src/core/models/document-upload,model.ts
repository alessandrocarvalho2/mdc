import AccountBalanceModel from "./bank.model";
import TransactionModel from "./filters/cash-flow-filter.model";

interface DonwloadUploadModel {
  id: number;
  filename: string;
  uploadedAt: Date;
  accountId: Date;
  accountBalance: AccountBalanceModel;
  transactions: TransactionModel[];
}

export default DonwloadUploadModel;
