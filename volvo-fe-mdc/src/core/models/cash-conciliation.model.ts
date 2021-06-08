import CashFlowModel from "./cash-flow.model";
import TransactionModel from "./transaction.model";

export interface CashConciliationModel {
    transactions?: TransactionModel[];
    cashFlows?: CashFlowModel[];
    date?: Date;
  }

  export default CashConciliationModel;
