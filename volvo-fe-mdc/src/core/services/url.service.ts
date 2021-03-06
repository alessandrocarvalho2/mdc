const url = {
  addr: {
    post: {
      LOGIN: "auth/token",
      ACCOUNT_BALANCE_SAVE: "accountBalance/save",
      BANK_STATEMENT_UPLOAD: "transaction/Upload",
      BANK_STATEMENT_INSERT: "transaction/insert",
      CASH_FLOW_SAVE: "cashFlow/Save",
      CASH_FLOW_UPLOAD: "cashFlow/UploadReceivables",
      CASH_FLOW_SAVE_ADJUSTMENT: "cashFlow/saveAdjustment",
      TRANSFER_BETWEEN_ACCOUNTS_SAVE: "report/SaveCashTransferReport",
      CASH_CONCiLIATION_SAVE: "cashFlow/saveConciliation",
      CASH_CONCiLIATION_UNDO: "cashFlow/undo",
      CASH_CONCiLIATION_UNDO_ALL: "cashFlow/undoAll",
      USER_INSERT: "/user/Insert",
    },
    get: {
      USER: "/user",
      USER_LIST: "/user/getList",
      ACCOUNT_BALANCE_LIST: "accountBalance/GetByAccount",
      BANK_ACCOUNT_LIST: "bankAccount/getAll",
      BANK_STATEMENT_ACCOUNT: "transaction/getByAccount",
      BANK_STATEMENT_LIST: "transaction/GetList",
      BANK_STATEMENT_ALLOW_SAVE: "accountBalance/isAllowedToSave",
      BANK_STATEMENT_SAVE: "accountBalance/isAllowedToSave",
      CASH_FLOW_LIST: "cashFlow/getList",
      CASH_FLOW_TOTALIZATION: "report/getTotalizationReport",
      CASH_FLOW_TEMPLATE: "cashFlow/TemplateReceivables",
      OPERATION_LIST_ALL: "operation/getAll",
      OPERATION_LIST: "operation/getList",
      CATEGORY_LIST_ALL: "category/getAll",
      CATEGORY_LIST: "category/getList",
      CASH_CONSOLIDATION_REPORT_LIST: "report/getCashConsolidationReport",
      TRANSFER_BETWEEN_ACCOUNTS_LIST: "report/getCashTransferReport",
      EXPORT_REPORT_KPI: "Export/ExportKPI",
      EXPORT_CONCILIATION_KPI: "Export/ExportConciliation",
      EXPORT_REPORT_OPERATIONALCF: "Export/ExportOperationalCF",
      EXPORT_REPORT_BANK_TRANSACTION: "Export/ExportCashFlow",
      MENU: "/user/GetByMenu",
    },
    delete: {
      //USER: '/Usuario/Delete/',
      BANK_STATEMENT_DELETE: "transaction/DeleteByBankAndDate",
      // ACOMPANHAMENTO: '/LogAcompanhamento/Excluir',
    },
  },
};

export default url;
