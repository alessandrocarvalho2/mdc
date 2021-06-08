

export interface CashAdjustmentModel{
    bankAccountId?: number,
    categoryId?: number,
    operationId?: number,
    amount?: number,
    inOut?: string,
    domainId?: number,
    description?: string,
  }

  export default CashAdjustmentModel;
