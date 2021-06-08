import React, { useEffect, useState } from "react";
import bankStatementListStyles from "./compare-list.style";
import BankStatementService from "../../../../core/services/bank-statement.service";
import {
  Box,
  Card,
  CardContent,
  Grid,
  Paper,
  Typography,
} from "@material-ui/core";
import Spinner from "../../../../shared/components/ecash-spinner/ecash-spinner.component";
import ComparaTableHeader from "./component/compare-table-header.component";
import CashFlowService from "../../../../core/services/cash-flow.service";
import CashFlowFilterModel from "../../../../core/models/filters/cash-flow-filter.model";
import CashList from "./component/cash-list/cash-list";
import StatementList from "./component/statement-list/statement-list";
import EcashTableBasicList from "../../../../shared/components/ecash-table-basic/ecash-table-basic.component";
import CashConciliationModel from "../../../../core/models/cash-conciliation.model";
import CashConsolidationService from "../../../../core/services/cash-conciliation.service";
import messageService from "../../../../core/services/message.service";
import SnackBarTypeEnum from "../../../../core/enum/snackbar-type.enum";
import EcashSnackbar from "../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import TransactionFilterModel from "../../../../core/models/filters/transaction-filter.model";
import CashConciliationService from "../../../../core/services/cash-conciliation.service";
import Columns from "../../../../core/models/columns-table.model";

const CompareList = () => {
  //let userAccess: any = localStorage.getItem("access");
  //userAccess = JSON.parse(userAccess);

  const bankStatementService = new BankStatementService();
  const cashFlowService = new CashFlowService();
  const cashConsolidationService = new CashConsolidationService();
  const classes = bankStatementListStyles(); // useState(bankStatementListStyles());
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");
  const [isPostBack, setisPostBack] = useState(false);
  const [dataStatement, setDataStatement] = useState<any>([]);
  const [dataCash, setDataCash] = useState<any>([]);
  const [dataStatementConciliation, setDataStatementConciliation] =
    useState<any>([]);
  const [dataCashConciliation, setDataCashConciliation] = useState<any>([]);
  const cashConciliationService = new CashConciliationService();

  const initialSearch = {
    bankAccountId: 0,
    date: new Date(),
  };
  const [search, setSearch] = useState<any>(initialSearch);

  const [summaryStatement, setSummaryStatement] = useState(0);
  const [summaryCash, setSummaryCash] = useState(0);
  const [summaryStatementConciliation, setSummaryStatementConciliation] =
    useState(0);
  const [summaryCashConciliation, setSummaryCashConciliation] = useState(0);

  const getList = () => {
    setLoading(true);
    setisPostBack(true);

    const filterCashFlow = {
      bankAccountId: search.bankAccount?.id,
      date: search.date,
      categoryId: search.category?.id,
      operationId: search.operation?.id,
      approved: search.approved?.id,
      inOut: search.inOut?.id,
      includeZeros: false,
      conciliated: false,
    } as CashFlowFilterModel;

    const filterTransaction = {
      bankAccountId: search.bankAccount?.id,
      date: search.date,
      conciliated: false,
    } as TransactionFilterModel;

    bankStatementService
      .getList(filterTransaction)
      .then((respBankStatement: any) => {
        setSummaryStatement(
          respBankStatement.reduce(
            (sum: number, current: any) => sum + (current ? current.amount : 0),
            0
          )
        );

        let tempDataStatement = respBankStatement;
        let tempDataCash: any = [];
        respBankStatement.forEach((element: any) => {
          element.checkedStatement = false;
        });

        cashFlowService
          .getList(filterCashFlow)
          .then((responseCash: any) => {
            setSummaryCash(
              responseCash.reduce(
                (sum: number, current: any) =>
                  sum + (current ? current.amount : 0),
                0
              )
            );
            responseCash.forEach((element: any) => {
              element.checkedCash = false;
            });
            tempDataCash = responseCash;
          })
          .finally(() => {
            tempDataStatement.forEach((elementStatement: any) => {
              tempDataCash.forEach((elementCash: any) => {
                if (!elementStatement.checkedStatement) {
                  if (!elementCash.checkedCash) {
                    if (
                      parseFloat(elementStatement.amount) ===
                      parseFloat(elementCash.amount)
                    ) {
                      elementStatement.checkedStatement = true;
                      elementCash.checkedCash = true;
                    }
                  }
                }
              });
            });
            getListConciliated();

            // tempDataStatement = tempDataStatement.sort((x: any, y: any) =>
            //   x.description > y.description ? 1 : -1
            // );

            let linhaStatement = 0;
            tempDataStatement.forEach((element: any) => {
              linhaStatement++;
              element.index = linhaStatement;
            });

            setDataStatement(tempDataStatement);

            tempDataCash = tempDataCash.sort((x: any, y: any) =>
              x.description > y.description ? 1 : -1
            );

            let linhaCash = 0;
            tempDataCash.forEach((element: any) => {
              linhaCash++;
              element.index = linhaCash;
            });

            setDataCash(tempDataCash);
          });
      })
      .catch((errors) => {
        console.error(errors);
        setLoading(false);
      })
      .finally(() => {});
  };

  const onSubmit = () => {
    getList();
  };

  const getListConciliated = () => {
    const filterCashFlow = {
      bankAccountId: search.bankAccount?.id,
      date: search.date,
      categoryId: search.category?.id,
      operationId: search.operation?.id,
      approved: search.approved?.id,
      inOut: search.inOut?.id,
      includeZeros: false,
      conciliated: true,
    } as CashFlowFilterModel;

    const filterTransaction = {
      bankAccountId: search.bankAccount?.id,
      date: search.date,
      conciliated: true,
    } as TransactionFilterModel;

    bankStatementService
      .getList(filterTransaction)
      .then((data: any) => {
        setSummaryStatementConciliation(
          data.reduce(
            (sum: number, current: any) => sum + (current ? current.amount : 0),
            0
          )
        );
        setDataStatementConciliation(
          data.sort((x: any, y: any) =>
            x.conciliationId > y.conciliationId ? 1 : -1
          )
        );

        setLoading(false);
      })
      .catch((errors) => {
        console.error(errors);
        setLoading(false);
      })
      .finally(() => {});

    cashFlowService
      .getList(filterCashFlow)
      .then((data: any) => {
        setSummaryCashConciliation(
          data.reduce(
            (sum: number, current: any) => sum + (current ? current.amount : 0),
            0
          )
        );
        setDataCashConciliation(
          data.sort((x: any, y: any) =>
            x.conciliationId > y.conciliationId ? 1 : -1
          )
        );
      })
      .finally(() => {});
  };

  const initialFields = {
    statementAmount: "",
    transactionAmount: "",
    balanceAmount: "",
    description: "",
  };

  const [fields, setFields] = useState(initialFields);

  const sumData = () => {
    let item = { ...fields };

    const itemsStatement = dataStatement.filter((x: any) => x.checkedStatement);
    const itemsCash = dataCash.filter((x: any) => x.checkedCash);

    const statementAmount = itemsStatement
      .filter((x: any) => x.checkedStatement)
      .reduce(
        (sum: number, current: any) => sum + (current ? current.amount : 0),
        0
      );

    const transactionAmount = itemsCash
      .filter((x: any) => x.checkedCash)
      .reduce(
        (sum: number, current: any) => sum + (current ? current.amount : 0),
        0
      );

    const balance =
      parseFloat(statementAmount.toFixed(2) ?? 0) -
      parseFloat(transactionAmount.toFixed(2) ?? 0);

    let description = "";
    if (
      itemsStatement &&
      itemsCash &&
      (itemsStatement.length > 0 || itemsCash.length > 0)
    ) {
      if (itemsStatement.length === 1 && itemsCash.length === 0) {
        description = itemsStatement[0].description;
      } else if (itemsStatement.length === 0 && itemsCash.length === 1) {
        description = itemsCash[0]?.domain?.description;
      }
    }

    setFields({
      ...item,
      statementAmount: statementAmount.toFixed(2),
      transactionAmount: transactionAmount.toFixed(2),
      balanceAmount: balance.toString(),
      description: description,
    });
  };

  const initialAddTransaction = {
    bankAccount: "",
    category: "",
    operation: "",
    description: "",
    banance: "",
  };

  const [addTransaction, setAddTransaction] = useState(initialAddTransaction);

  const onSaveConciliation = () => {
    const itemsStatement = dataStatement.filter((x: any) => x.checkedStatement);
    const itemsCash = dataCash.filter((x: any) => x.checkedCash);

    if (itemsStatement.length > 0 || itemsCash.length > 0) {
      const item = {
        transactions: itemsStatement,
        cashFlows: itemsCash,
        date: new Date(search.date.toDateString()),
      } as CashConciliationModel;
      save(item);
    } else
      snackBarOpen(
        messageService.info.MSG006.replace("{0}", "Conciliação"),
        SnackBarTypeEnum.WARNING
      );
  };

  const [snackOpen, setSnackOpen] = useState(false);
  const [typeSnack, setTypeSnack] = useState(SnackBarTypeEnum.INFO);

  const snackBarOpen = (message: string, type: SnackBarTypeEnum) => {
    setSnackOpen(true);
    setMessage(message);
    setTypeSnack(type);
  };

  const save = (item: CashConciliationModel) => {
    setLoading(true);
    setisPostBack(true);

    cashConsolidationService
      .save(item)
      .then((resp: any) => {
        onSubmit();
        setLoading(false);
        snackBarOpen(
          messageService.success.MSG003.replace("{0}", "Conciliação"),
          SnackBarTypeEnum.SUCCESS
        );
      })
      .catch((error) => {
        setLoading(false);
        if (error.response) {
          if (error.response.data !== "") {
            snackBarOpen(
              error.response.data.toString(),
              SnackBarTypeEnum.ERROR
            );
          } else
            snackBarOpen(
              messageService.error.MSG003.replace("{0}", "Conciliação"),
              SnackBarTypeEnum.ERROR
            );
        } else if (error.request) {
          snackBarOpen(
            messageService.error.MSG003.replace("{0}", "Conciliação"),
            SnackBarTypeEnum.ERROR
          );
        } else {
          snackBarOpen(
            messageService.error.MSG003.replace("{0}", "Conciliação"),
            SnackBarTypeEnum.ERROR
          );
          // Something happened in setting up the request and triggered an Error
          console.log("Error", error.message);
        }
      })
      .finally(() => {});
  };

  const undo = () => {
    cashConciliationService
      .undo(search?.bankAccount.id, search?.date)
      .then((resp: any) => {
        onSubmit();
        snackBarOpen(
          messageService.success.MSG003.replace("{0}", "Conciliação"),
          SnackBarTypeEnum.SUCCESS
        );
      })
      .catch((error: any) => {
        setLoading(false);
        if (error.response) {
          if (error.response.data !== "") {
            snackBarOpen(
              error.response.data.toString(),
              SnackBarTypeEnum.ERROR
            );
          } else
            snackBarOpen(
              messageService.error.MSG007.replace("{0}", "Conciliação"),
              SnackBarTypeEnum.ERROR
            );
        } else if (error.request) {
          snackBarOpen(
            messageService.error.MSG007.replace("{0}", "Conciliação"),
            SnackBarTypeEnum.ERROR
          );
        } else {
          snackBarOpen(
            messageService.error.MSG007.replace("{0}", "Conciliação"),
            SnackBarTypeEnum.ERROR
          );
          // Something happened in setting up the request and triggered an Error
          console.log("Error", error.message);
        }
      })
      .finally(() => {});
  };

  const undoAll = () => {
    cashConciliationService
      .undoAll(search?.bankAccount.id, search?.date)
      .then((resp: any) => {
        onSubmit();
        snackBarOpen(
          messageService.success.MSG003.replace("{0}", "Conciliação"),
          SnackBarTypeEnum.SUCCESS
        );
      })
      .catch((error: any) => {
        setLoading(false);
        if (error.response) {
          if (error.response.data !== "") {
            snackBarOpen(
              error.response.data.toString(),
              SnackBarTypeEnum.ERROR
            );
          } else
            snackBarOpen(
              messageService.error.MSG007.replace("{0}", "Conciliação"),
              SnackBarTypeEnum.ERROR
            );
        } else if (error.request) {
          snackBarOpen(
            messageService.error.MSG007.replace("{0}", "Conciliação"),
            SnackBarTypeEnum.ERROR
          );
        } else {
          snackBarOpen(
            messageService.error.MSG007.replace("{0}", "Conciliação"),
            SnackBarTypeEnum.ERROR
          );
          // Something happened in setting up the request and triggered an Error
          console.log("Error", error.message);
        }
      })
      .finally(() => {});
  };

  let columnsStatementConciliation: Columns[] = [
    {
      id: "conciliationId",
      label: "ID",
      align: "left",
      type: "string",
      width: "7",
    },
    // {
    //   id: "bankAccount.nickname",
    //   label: "Conta",
    //   align: "left",
    //   type: "string",
    //   width: "8",
    // },
    {
      id: "description",
      label: "Descrição",
      align: "left",
      type: "string",
      width: "58",
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
      type: "string",
      applyColorMonetary: true,
      width: "35",
    },
  ];

  let columnsCashConciliation: Columns[] = [
    {
      id: "conciliationId",
      label: "ID",
      align: "left",
      type: "string",
      width: "7",
    },
    {
      id: "domain.operation.code",
      label: "Operação",
      align: "left",
      type: "string",
      width: "8",
    },
    {
      id: "description",
      label: "Movimentação",
      align: "left",
      type: "string",
      width: "50",
    },

    {
      id: "amount",
      label: "Valor",
      align: "right",
      type: "string",
      format: (value: any) =>
        value.toLocaleString("pt-BR", {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2,
        }),
      applyColorMonetary: true,
      width: "35",
    },
  ];

  useEffect(() => {
    if (isPostBack) sumData();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [dataStatement, dataCash]);

  return (
    <>
      <div className={classes.root}>
        <ComparaTableHeader
          onSubmit={onSubmit}
          onSaveConciliation={onSaveConciliation}
          dataCash={dataCash}
          dataStatement={dataStatement}
          setDataCash={setDataCash}
          setDataStatement={setDataStatement}
          search={search}
          setSearch={setSearch}
          fields={fields}
          setFields={setFields}
          addTransaction={addTransaction}
          setAddTransaction={setAddTransaction}
          undo={undo}
          undoAll={undoAll}
        />
        {(() => {
          if (loading) {
            return <Spinner />;
          }
          if (isPostBack) {
            return (
              <Box>
                <Paper>
                  <Card className={classes.root}>
                    <CardContent>
                      <Typography
                        align="center"
                        color="secondary"
                        gutterBottom
                        variant="h5"
                        component="h2"
                      >
                        Itens para Conciliar
                      </Typography>
                      <Grid
                        container
                        spacing={2}
                        alignItems="flex-start"
                        justify="center"
                      >
                        <Grid item md={6}>
                          <StatementList
                            totalAmount={summaryStatement}
                            data={dataStatement}
                            setData={setDataStatement}
                          />
                        </Grid>
                        <Grid item md={6}>
                          <CashList
                            data={dataCash}
                            setData={setDataCash}
                            totalAmount={summaryCash}
                          />
                        </Grid>
                      </Grid>
                    </CardContent>
                  </Card>
                </Paper>
                <Paper>
                  <Card className={classes.root}>
                    <CardContent>
                      <Typography
                        align="center"
                        color="secondary"
                        gutterBottom
                        variant="h5"
                        component="h2"
                      >
                        Itens Conciliados
                      </Typography>
                      <Grid
                        container
                        spacing={2}
                        alignItems="flex-start"
                        justify="center"
                      >
                        <Grid item md={6}>
                          <EcashTableBasicList
                            totalAmount={summaryStatementConciliation}
                            data={dataStatementConciliation}
                            //setData={setDataStatementConciliation}
                            columns={columnsStatementConciliation}
                            titleTable={"EXTRATO BANCÁRIO CONCILIADO"}
                          />
                        </Grid>
                        <Grid item md={6}>
                          <EcashTableBasicList
                            totalAmount={summaryCashConciliation}
                            data={dataCashConciliation}
                            //setData={setDataCashConciliation}
                            columns={columnsCashConciliation}
                            titleTable={"EXTRATO ECASH CONCILIADO"}
                          />
                        </Grid>
                      </Grid>
                    </CardContent>
                  </Card>
                </Paper>
              </Box>
            );
          }
        })()}
        <EcashSnackbar
          open={snackOpen}
          setOpen={setSnackOpen}
          type={typeSnack}
          message={message}
        />
      </div>
    </>
  );
};

export default CompareList;
