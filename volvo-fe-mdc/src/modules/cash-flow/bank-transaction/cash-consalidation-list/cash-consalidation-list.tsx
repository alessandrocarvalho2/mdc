import {
  Grid,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
} from "@material-ui/core";
import React, { useEffect, useState } from "react";
import CashConsolidationReportModel from "../../../../core/models/cash-consolidation-report.model";
import ReportService from "../../../../core/services/report.service";
import Spinner from "../../../../shared/components/ecash-spinner/ecash-spinner.component";
import bankTransactionListStyles from "./cash-consalidation-list.style";

interface Column {
  id:
    | "id"
    | "bankAccount.bank.bankCode"
    | "bankAccount.bank.bankName"
    | "bankAccount.agency"
    | "bankAccount.account"
    | "bankAccount.accountBalance.balance"
    | "bankAccount.nickname"
    | "credits"
    | "debits"
    | "reserveAmount"
    | "minimumBalance";
  label: string;
  width?: string;
  align?: "left" | "center" | "right";
  type?: "string" | "textfield" | "checkbox";
  format?: (value: any) => string;
}

let columns: Column[] = [
  {
    id: "bankAccount.bank.bankCode",
    label: "ID",
    align: "left",
    type: "string",
  },
  {
    id: "bankAccount.nickname",
    label: "Banco",
    align: "left",
    type: "string",
  },
  {
    id: "bankAccount.agency",
    label: "Agência",
    align: "left",
    type: "string",
    //format: (value: any) => moment(new Date(value)).format("DD/MM/YYYY"),
  },
  {
    id: "bankAccount.account",
    label: "Conta",
    align: "left",
    type: "string",
    //format: (value: any) => moment(new Date(value)).format("DD/MM/YYYY"),
  },
  {
    id: "bankAccount.accountBalance.balance",
    label: "Saldo Atual",
    align: "right",
    type: "string",
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
  {
    id: "credits",
    label: "Crédito",
    align: "right",
    type: "string",
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
  {
    id: "debits",
    label: "Débito",
    align: "right",
    type: "string",
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
  {
    id: "reserveAmount",
    label: "Saldo Reserva",
    align: "right",
    type: "string",
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
  {
    id: "minimumBalance",
    label: "Saldo Mínimo",
    align: "right",
    type: "string",
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
];

const CashConsalidationList = () => {
  const [isPostBack, setIsPostBack] = useState(false);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [total, setTotal] = useState(0);
  const [data, setData] = useState<any>();
  const [
    cashConsolidationReport,
    setCashConsolidationReport,
  ] = useState<CashConsolidationReportModel>();

  const classes = bankTransactionListStyles();
  const reportService = new ReportService();

  const totalColumn = columns.length;
  const width = 100 / totalColumn;

  columns.forEach((element) => {
    element.width = width.toString() + "%";
  });

  const getList = () => {
    setLoading(true);

    reportService
      .getCashConsolidationReport(new Date())
      .then((response: any) => {
        const cashConsolidationReport = response;
        const cashConsolidationItems = response.cashConsolidationItems;
        setCashConsolidationReport(cashConsolidationReport);
        setData(cashConsolidationItems);
        setTotal(cashConsolidationItems.length);
        setRowsPerPage(cashConsolidationItems.length);
        setIsPostBack(true);
        setLoading(false);
      })
      .catch((errors) => {
        setIsPostBack(false);
        console.error(errors);
        setLoading(false);
      });
  };


  useEffect(() => {
    getList();
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleChangePage = (event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  return (
    <>
      <div className={classes.root}>
        <Grid container spacing={4} alignItems="center" justify="space-between">
          <Grid item md={8}>
            <span className={classes.title}>Fechamento de Caixa</span>
          </Grid>
        </Grid>
        {(() => {
          if (loading) {
            return <Spinner />;
          }
          if (isPostBack) {
            if (data?.length === 0) {
              return (
                <Paper>
                  <TableContainer className={classes.containertable}>
                    <Table size={"small"} stickyHeader aria-label="sticky table">
                      <TableBody>
                        <TableRow>
                          <TableCell align="center" colSpan={8}>
                            Resultado não encontrado.
                          </TableCell>
                        </TableRow>
                      </TableBody>
                    </Table>
                  </TableContainer>
                </Paper>
              );
            } else {
              return (
                <Paper>
                  {" "}
                  <TableContainer className={classes.containertable}>
                    <Table stickyHeader size={'small'} aria-label="sticky table">
                      <TableHead>
                        <TableRow>
                          {columns.map((column: any, index: any) => {
                            return (
                              <TableCell
                                className={classes.head}
                                key={column.id}
                                align={column.align}
                                style={{ width: column.width }}
                              >
                                {column.label}
                              </TableCell>
                            );
                          })}
                        </TableRow>
                      </TableHead>
                      <TableBody>
                        {data
                          ?.slice(
                            page * rowsPerPage,
                            page * rowsPerPage + rowsPerPage
                          )
                          .map((row: any, index: any) => {
                            return (
                              <TableRow
                                hover
                                role="checkbox"
                                tabIndex={-1}
                                key={index}
                                className={
                                  row?.bankAccount?.isMainAccount
                                    ? classes.backgroundColorRowMain
                                    : ""
                                }
                              >
                                {columns.map((column) => {
                                  console.log(row.bankAccount.isMainAccount);
                                  let value = row[column.id];
                                  let names = column.id.split(".");
                                  if (data.length > 0 && names.length === 2) {
                                    const obj = row[names[0]][names[1]];
                                    value = obj;
                                  } else if (
                                    data.length > 0 &&
                                    names.length === 3
                                  ) {
                                    const obj =
                                      row[names[0]][names[1]][names[2]];
                                    value = obj;
                                  }
                                  const classeText =
                                    column.id === "debits" ||
                                    column.id === "credits" ||
                                    column.id === "minimumBalance" ||
                                    column.id === "reserveAmount" ||
                                    column.id ===
                                      "bankAccount.accountBalance.balance"
                                      ? value < 0
                                        ? classes.cellTextRed
                                        : classes.cellTextBlue
                                      : classes.cellText;

                                  return (
                                    <TableCell
                                      className={classeText}
                                      key={column.id}
                                      align={column.align}
                                    >
                                      {column.format
                                        ? column.format(value)
                                        : value}
                                    </TableCell>
                                  );
                                })}
                              </TableRow>
                            );
                          })}
                      </TableBody>
                    </Table>
                  </TableContainer>
                  <TableContainer className={classes.containertable}>
                    <Table size={"small"} stickyHeader aria-label="sticky table">
                      <TableBody>
                        <TableRow>
                          <TableCell
                            component="th"
                            align="right"
                            style={{ width: width.toString() + "%" }}
                          >
                            {" "}
                          </TableCell>
                          <TableCell
                            component="th"
                            align="right"
                            style={{ width: width.toString() + "%" }}
                          >
                            {" "}
                          </TableCell>
                          <TableCell
                            component="th"
                            align="right"
                            style={{ width: width.toString() + "%" }}
                          >
                            {" "}
                          </TableCell>
                          <TableCell
                            component="th"
                            align="right"
                            style={{ width: width.toString() + "%" }}
                          >
                            {"TOTAL"}
                          </TableCell>
                          <TableCell
                            component="th"
                            align="right"
                            style={{ width: width.toString() + "%" }}
                            className={
                              cashConsolidationReport?.totalAmount !==
                                undefined &&
                              cashConsolidationReport?.totalAmount >= 0
                                ? classes.cellTextBlue
                                : classes.cellTextRed
                            }
                          >
                            {cashConsolidationReport?.totalAmount?.toLocaleString(
                              "pt-BR",
                              {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2,
                              }
                            )}
                          </TableCell>
                          <TableCell
                            component="th"
                            align="right"
                            style={{ width: width.toString() + "%" }}
                            className={
                              cashConsolidationReport?.totalCredits !==
                                undefined &&
                              cashConsolidationReport?.totalCredits >= 0
                                ? classes.cellTextBlue
                                : classes.cellTextRed
                            }
                          >
                            {cashConsolidationReport?.totalCredits?.toLocaleString(
                              "pt-BR",
                              {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2,
                              }
                            )}
                          </TableCell>
                          <TableCell
                            component="th"
                            align="right"
                            style={{ width: width.toString() + "%" }}
                            className={
                              cashConsolidationReport?.totalDebits !==
                                undefined &&
                              cashConsolidationReport?.totalDebits >= 0
                                ? classes.cellTextBlue
                                : classes.cellTextRed
                            }
                          >
                            {cashConsolidationReport?.totalDebits?.toLocaleString(
                              "pt-BR",
                              {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2,
                              }
                            )}
                          </TableCell>
                          <TableCell
                            component="th"
                            align="right"
                            style={{ width: width.toString() + "%" }}
                            className={
                              cashConsolidationReport?.totalReserve !==
                                undefined &&
                              cashConsolidationReport?.totalReserve >= 0
                                ? classes.cellTextBlue
                                : classes.cellTextRed
                            }
                          >
                            {cashConsolidationReport?.totalReserve?.toLocaleString(
                              "pt-BR",
                              {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2,
                              }
                            )}
                          </TableCell>
                          <TableCell
                            component="th"
                            align="right"
                            style={{ width: width.toString() + "%" }}
                            className={
                              cashConsolidationReport?.totalMinimum !==
                                undefined &&
                              cashConsolidationReport?.totalMinimum >= 0
                                ? classes.cellTextBlue
                                : classes.cellTextRed
                            }
                          >
                            {cashConsolidationReport?.totalMinimum?.toLocaleString(
                              "pt-BR",
                              {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2,
                              }
                            )}
                          </TableCell>
                        </TableRow>
                        <TableRow
                          className={
                            cashConsolidationReport?.applicationAmount !==
                              undefined &&
                            cashConsolidationReport?.applicationAmount > 0
                              ? classes.backgroundColorRowApply
                              : cashConsolidationReport?.applicationAmount === 0
                              ? ""
                              : classes.backgroundColorRowRescue
                          }
                        >
                          <TableCell
                            align="right"
                            style={{ width: width.toString() + "%" }}
                          >
                            {" "}
                          </TableCell>
                          <TableCell
                            align="right"
                            style={{ width: width.toString() + "%" }}
                          >
                            {" "}
                          </TableCell>
                          <TableCell
                            align="right"
                            style={{ width: width.toString() + "%" }}
                          >
                            {" "}
                          </TableCell>
                          <TableCell
                            align="right"
                            colSpan={4}
                            className={
                              cashConsolidationReport?.applicationAmount !==
                                undefined &&
                              cashConsolidationReport?.applicationAmount > 0
                                ? classes.cellTextWhite
                                : cashConsolidationReport?.applicationAmount ===
                                  0
                                ? classes.cellText
                                : classes.cellTextWhite
                            }
                            style={{ width: (width * 4).toString() + "%" }}
                          >
                            {"Volvo BR03 - "} {cashConsolidationReport?.message}
                          </TableCell>

                          <TableCell
                            align="right"
                            colSpan={2}
                            style={{ width: (width * 2).toString() + "%" }}
                            className={
                              cashConsolidationReport?.applicationAmount !==
                                undefined &&
                              cashConsolidationReport?.applicationAmount > 0
                                ? classes.cellTextWhite
                                : cashConsolidationReport?.applicationAmount ===
                                  0
                                ? classes.cellText
                                : classes.cellTextWhite
                            }
                          >
                            {cashConsolidationReport?.applicationAmount?.toLocaleString(
                              "pt-BR",
                              {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2,
                              }
                            )}
                          </TableCell>
                        </TableRow>
                      </TableBody>
                    </Table>
                  </TableContainer>
                  <TablePagination
                    className={classes.head}
                    rowsPerPageOptions={[
                      5,
                      10,
                      25,
                      { label: "All", value: data.length },
                    ]}
                    component="div"
                    count={total}
                    rowsPerPage={rowsPerPage}
                    page={page}
                    onChangePage={handleChangePage}
                    onChangeRowsPerPage={handleChangeRowsPerPage}
                  />
                </Paper>
              );
            }
          }
          <></>;
        })()}
      </div>
    </>
  );
};

export default CashConsalidationList;
