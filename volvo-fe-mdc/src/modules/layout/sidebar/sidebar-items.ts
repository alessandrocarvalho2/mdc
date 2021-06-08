import { sidebarItem } from "./sidebar.interface";

const sidebarItems: sidebarItem[] = [
  {
    title: "Gerenciar",
    url: "setting",
    icon: "settings",
    access: ["DOMAIN_VISUALIZAR", "USUARIO_VISUALIZAR"],
  },
  {
    title: "Conciliação Bancária",
    url: "bank-statement",
    icon: "playlist_add_check",
    access: ["BANK_STATEMENT_VISUALIZAR", "COMPARE_VISUALIZAR"],
  },
  {
    title: "Fluxo de caixa",
    url: "bank-transaction",
    icon: "transform",
    access: ["BANK_TRANSACTION_VISUALIZAR", "CASH_CONSOLALIDATION_VISUALIZAR","TRANSFER-BETWEEN-ACCOUNTS-VISUALIZAR"],
  },
  {
    title: "Relatórios",
    url: "reports",
    icon: "timeline",
    access: ["REPORTS_VISUALIZAR"],
  }
];

export default sidebarItems;
