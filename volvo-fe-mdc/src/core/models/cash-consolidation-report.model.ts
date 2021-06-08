import CashConsolidationItemModel from "./cash-consolidation-item.model";

export interface CashConsolidationReportModel {
    id?: number;
    cashConsolidationItems?: CashConsolidationItemModel[];
    totalAmount?: number;
    totalCredits?: number;
    totalDebits?: number;
    totalMinimum?: number;
    totalReserve?: number;
    message?: string;
    applicationAmount?: number;
 
  }

  export default CashConsolidationReportModel;
