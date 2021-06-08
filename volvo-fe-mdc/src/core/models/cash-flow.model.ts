import CashFlowDetailedModel from "./cash-flow-detailed.model";
import DomainModel from "./domain.model";

export interface CashFlowModel {
    id?: number;
    date?: Date;
    approval?: boolean;
    domainId?: number;
    domain?: DomainModel;
    amount?: number;
    manualAdjustment?: boolean;
    isDetailedTransaction?: boolean;
    conciliationId?: number;
    description?: string;
    createAt?: Date;
    createBy?: number;
    updateAt?: Date;
    updateBy?: number;
    cashFlowDetaileds: CashFlowDetailedModel[];
 
  }

  export default CashFlowModel;