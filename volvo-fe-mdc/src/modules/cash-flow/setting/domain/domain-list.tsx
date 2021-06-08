import React from "react";
import Columns from "../../../../core/models/columns-table.model";

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let columns: Columns[] = [
  {
    id: "category.description",
    label: "Categoria",
    align: "left",
    type: "string",
  },
  {
    id: "description",
    label: "Descrição",
    align: "left",
    type: "string",
  },
  {
    id: "domain.inOut",
    label: "IN/OUT",
    align: "center",
    type: "string",
  },
  {
    id: "domain.operation.code",
    label: "Operação",
    align: "left",
    type: "string",
  },
  {
    id: "bankAccountId",
    label: "Conta",
    align: "left",
    type: "string",
  },
  {
    id: "Necess. Aprov?",
    label: "Conta",
    align: "left",
    type: "string",
    format: (value: any) => (value ? "Sim" : "nÃO"),
  },
  {
    id: "Necess. Aprov?",
    label: "Conta",
    align: "left",
    type: "string",
    format: (value: any) => (value ? "Sim" : "nÃO"),
  },
  {
    id: "amount",
    label: "Valor ",
    align: "right",
    type: "string",
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
  {
    id: "detailedDescription",
    label: "Observações",
    align: "left",
    type: "string",
  },
  {
    id: "index",
    label: "Ações",
    align: "center",
    type: "button",
    children: [
      {
        id: "isDetailedTransaction",
        label: "Editar",
        align: "center",
        type: "button",
        icon: "edit",
      },
    ],
  },
];

const DomainList = () => {
  return <div></div>;
};

export default DomainList;
