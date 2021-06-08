import React, { useState } from "react";
import bankStatementListStyles from "./bank-statement-list.style";
import BankStatementService from "../../../../core/services/bank-statement.service";
import BankStatementHeader from "./components/bank-statement-table-header.component";
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
} from "@material-ui/core";
import LayoutService from "../../../../core/services/layout.service";
import Spinner from "../../../../shared/components/ecash-spinner/ecash-spinner.component";
import moment from "moment";
import AccountBalanceService from "../../../../core/services/account-balance.service";
import TransactionFilterModel from "../../../../core/models/filters/transaction-filter.model";

interface Column {
  id: "id" | "bankAccount.nickname" | "date" | "description" | "amount";
  label: string;
  minWidth?: number;
  align?: "right" | "center" | "left";
  format?: (value: any) => string;
}

let columns: Column[] = [
  {
    id: "bankAccount.nickname",
    label: "Conta Corrente",
    align: "left",
  },
  {
    id: "date",
    label: "Date",
    align: "center",
    format: (value: any) => moment(new Date(value)).format("DD/MM/YYYY"),
  },
  {
    id: "description",
    label: "Descrição",
    align: "left",
  },
  {
    id: "amount",
    label: "Valor",
    align: "right",
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
];

export default function BankStatementList() {
  //let userAccess: any = localStorage.getItem("access");
  //userAccess = JSON.parse(userAccess);

  const bankStatementService = new BankStatementService();
  const accountBalanceService = new AccountBalanceService();
  const classes = bankStatementListStyles(); // useState(bankStatementListStyles());
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [total, setTotal] = useState(0);
  const [accountBalance, setAccountBalance] = useState(0);
  const [isPosBack, setIspPosBack] = useState(false);

  const [search, setSearch] = useState<any>({
    bankAccount: 0,
    date: new Date(),
  });

  const minWidth = LayoutService.getHeightTable() / columns.length;

  columns.forEach((element) => {
    element.minWidth = minWidth;
  });

  const [bankStatementList, setBankStatementList] = useState<any>([]);

  const getListStatement = (bankStatementId: number, date: Date) => {
    setLoading(true);
    setIspPosBack(true);

    accountBalanceService
      .getList(bankStatementId, date)
      .then((data: any) => {
        if (data.length > 0) {
          setAccountBalance(data[0].balance);
        } else {
          setAccountBalance(0);
        }
      })
      .catch((errors) => {
        console.error(errors);
      });

    const filterTransaction = {
      bankAccountId: bankStatementId,
      date: date,
    } as TransactionFilterModel;

    bankStatementService
      .getList(filterTransaction)
      .then((data: any) => {
        setTotal(data.length);
        setRowsPerPage(data.length)
        setBankStatementList(data);
        setLoading(false);
      })
      .catch((errors) => {
        console.error(errors);
        setLoading(false);
      });
  };

  const onSubmit = () => {
    getListStatement(search.bankAccount, search.date);
  };

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
        <BankStatementHeader
          onSubmit={onSubmit}
          search={search}
          setSearch={setSearch}
        />
        {(() => {
          if (loading) {
            return <Spinner />;
          }
          if (isPosBack) {
            return (
              <Paper>
                <TableContainer className={classes.containertable}>
                  <Table size={"small"} stickyHeader aria-label="sticky table">
                    <TableHead>
                      <TableRow>
                        {columns.map((column) => (
                          <TableCell
                            className={classes.head}
                            key={column.id}
                            align={column.align}
                            style={{ minWidth: column.minWidth }}
                          >
                            {column.label}
                          </TableCell>
                        ))}
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {bankStatementList
                        .slice(
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
                            >
                              {columns.map((column) => {
                                let value = row[column.id];
                                let names = column.id.split(".");
                                if (names.length > 1) {
                                  const obj = row[names[0]][names[1]];
                                  value = obj;
                                }

                                if (column.id === "amount") {
                                  const classeText =
                                    column.id === "amount"
                                      ? parseFloat(value) < 0
                                        ? classes.cellTextRed
                                        : classes.cellTextBlue
                                      : classes.cellTextRed;
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
                                } else {
                                  return (
                                    <TableCell
                                      className={classes.cellText}
                                      key={column.id}
                                      align={column.align}
                                    >
                                      {column.format
                                        ? column.format(value)
                                        : value}
                                    </TableCell>
                                  );
                                }
                              })}
                            </TableRow>
                          );
                        })}
                      {bankStatementList.length === 0 && (
                        <TableRow>
                          <TableCell align="center" colSpan={4}>
                            Resultado não encontrado.
                          </TableCell>
                        </TableRow>
                      )}
                    </TableBody>
                  </Table>
                </TableContainer>
                <TableContainer className={classes.containertable}>
                  <Table size={"small"} stickyHeader aria-label="sticky table">
                    <TableHead>
                      {(() => {
                        const classeText =
                          accountBalance < 0
                            ? classes.cellTextRed
                            : classes.cellTextBlue;
                        return (
                          <TableRow>
                            <TableCell
                              align="center"
                              style={{ minWidth: minWidth }}
                              colSpan={2}
                            >
                              SALDO CONTA:
                            </TableCell>
                            <TableCell
                              align="center"
                              style={{ minWidth: minWidth }}
                              colSpan={2}
                              className={classeText}
                            >
                              {" "}
                              {accountBalance.toLocaleString("pt-BR", {
                                minimumFractionDigits: 2,
                                maximumFractionDigits: 2,
                              })}
                            </TableCell>
                          </TableRow>
                        );
                      })()}
                    </TableHead>
                  </Table>
                </TableContainer>
                <TablePagination
                  rowsPerPageOptions={[
                    5,
                    10,
                    25,
                    { label: "All", value: bankStatementList.length },
                  ]}
                  component="div"
                  count={total}
                  className={classes.head}
                  rowsPerPage={rowsPerPage}
                  page={page}
                  onChangePage={handleChangePage}
                  onChangeRowsPerPage={handleChangeRowsPerPage}
                />
              </Paper>
            );
          }
        })()}

      </div>
    </>
  );
}
