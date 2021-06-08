import { subHeaderKey } from './sub-header.interface';

const subHeaderTabs: subHeaderKey = {
  "setting": [
    {
      "title": "Manter Domínio",
      "url": "domain_list",
      "access": "DOMAIN_VISUALIZAR"
    },
  ],
  "bank-statement": [
    {
      "title": "Extrato Bancário",
      "url": "bank-statement-list",
      "access": "BANK_STATEMENT_VISUALIZAR"
    },
    {
      "title": "Conciliação",
      "url": "compare-list",
      "access": "COMPARE_VISUALIZAR"
    },
  ],
  "bank-transaction": [
    {
      "title": "Movimentação de caixa",
      "url": "bank-transaction-list",
      "access": "BANK_TRANSACTION_VISUALIZAR"
    },
    {
      "title": "Fechamento de caixa",
      "url": "cash-consalidation-list",
      "access": "CASH_CONSOLALIDATION_VISUALIZAR"
    },
    {
      "title": "Transferências entre contas",
      "url": "transfer-between-accounts-list",
      "access": "TRANSFER-BETWEEN-ACCOUNTS-VISUALIZAR"
    },
    
  ],
  "reports": [
    {
      "title": "Saldos bancários",
      "url": "report-kpi",
      "access": "BANK_TRANSACTION_VISUALIZAR"
    },
    {
      "title": "Resumo da conciliação bancária",
      "url": "report-conciliation",
      "access": "BANK_TRANSACTION_VISUALIZAR"
    },
    {
      "title": "Fluxo de caixa realizado",
      "url": "report-operationalcf",
      "access": "BANK_TRANSACTION_VISUALIZAR"
    },
    
  ],
}

export default subHeaderTabs
