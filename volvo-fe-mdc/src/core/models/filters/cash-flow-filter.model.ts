
interface CashFlowFilterModel {
  id?: number;
  bankAccountId?: number;
  operationId?: number;
  domainId?: number;
  categoryId?: number;
  description?: string;
  inOut?: string;
  date?: Date;
  approved?: number;
  visible?: boolean;
  includeZeros?: boolean;
  conciliated?: boolean;
}

export default CashFlowFilterModel;
