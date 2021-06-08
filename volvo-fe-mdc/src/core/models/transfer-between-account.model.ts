import BankAccountModel from "./bank-account.interface";

interface TransferBetweenAccountModel {
  originAccount: BankAccountModel;
  destinyAccount: BankAccountModel;
  amountToTransfer: number;
}

export default TransferBetweenAccountModel;
