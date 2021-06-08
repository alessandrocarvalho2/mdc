
export interface CashFlowDetailedModel {
    id?: number;
    cashFlowId?: number;
    documentName?: string;
    amount?: number;
    detailedDescription?: string;
    delete?: boolean;
    isChanged?: boolean;
  }

  export default CashFlowDetailedModel;