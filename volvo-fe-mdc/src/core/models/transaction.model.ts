import BankAccountModel from "./bank-account.interface";

interface TransactionModel {
  id?: number;
  bankAccountId?: number;
  operationId?: number;
  domainId?: number;
  categoryId?: number;
  description?: string;
  inOut?: string;
  distortion?: boolean;
  amount?: number;
  date?: Date;
  approved?: boolean;
  documentUploadId?: number;
  bankAccount?: BankAccountModel;
}

export default TransactionModel;
