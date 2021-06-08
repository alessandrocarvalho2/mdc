import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Button,
  Checkbox,
  FormControl,
  FormControlLabel,
  FormGroup,
  Grid,
  Icon,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TablePagination,
  TableRow,
  TextField,
  Tooltip,
  Typography,
} from "@material-ui/core";
import ExpandMoreIcon from "@material-ui/icons/ExpandMore";
import React, { useEffect, useState } from "react";
import SnackBarTypeEnum from "../../../../core/enum/snackbar-type.enum";
import ModalTypeEnum from "../../../../core/enum/modal-type.enum";
import CashFlowModel from "../../../../core/models/cash-flow.model";
import CashFlowService from "../../../../core/services/cash-flow.service";
import ActionModal from "../../../../shared/components/ecash-action-modal/action-modal.component";
import NumberFormatCustomWithOutNegative from "../../../../shared/components/ecash-number-format/number-format-custom-without-negative.component";
import EcashSnackbar from "../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import Spinner from "../../../../shared/components/ecash-spinner/ecash-spinner.component";
import bankTransactionListStyles from "./bank-transaction-list.style";
import BankTransactionTableHeader from "./components/bank-transaction-table-header.component";
import messageService from "../../../../core/services/message.service";
import TransactionFilterModel from "../../../../core/models/filters/cash-flow-filter.model";
import EcashTablePaginationActions from "../../../../shared/components/ecash-table-pagination-actions/ecash-table-pagination-actions";
import TotalizationList from "./components/totalization-list/totalization-list";
import validation from "../../../../core/utils/validation.util";
import ReportManualPaymentsModal from "./components/report-manual-payments-modal/report-manual-payments-modal.component";
import ExportService from "../../../../core/services/export.service";
import CashFlowFilterModel from "../../../../core/models/filters/cash-flow-filter.model";

interface Column {
  id:
    | "id"
    | "domain.category.description"
    | "description"
    | "domain.description"
    | "domain.inOut"
    | "domain.operation.code"
    | "domain.isDetailedTransaction"
    | "amount"
    | "approval";
  label?: string;
  minWidth?: string;
  icon?: string;
  align?: "left" | "center" | "right";
  type?: "string" | "textfield" | "checkbox" | "button";
  witnCheckedAll?: boolean;
  format?: (value: any) => string;
  children?: Column[];
}

let columns: Column[] = [
  {
    id: "domain.category.description",
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
    id: "amount",
    label: "Valor",
    align: "right",
    type: "textfield",
  },
  {
    id: "approval",
    label: "Aprovação",
    align: "center",
    type: "checkbox",
  },
  {
    id: "domain.isDetailedTransaction",
    label: "Ações",
    align: "center",
    type: "button",
    children: [
      {
        id: "id",
        label: "Datelhe",
        align: "center",
        type: "button",
        icon: "",
      },
    ],
  },
];

interface ColumnTotalization {
  id: string;
  label: string;
  applyColorMonetary?: boolean;
  minWidth?: string;
  align?: "left" | "center" | "right";
  type?: "index" | "string" | "textfield" | "checkbox" | "monetary";
  format?: (value: any) => string;
}

let columnsIn: ColumnTotalization[] = [
  {
    id: "categoryName",
    label: "Categoria",
    align: "left",
    type: "string",
  },
  {
    id: "subTotal",
    label: "Total",
    align: "right",
    type: "string",
    applyColorMonetary: true,
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
];

let columnsOut: ColumnTotalization[] = [
  {
    id: "categoryName",
    label: "Categoria",
    align: "left",
    type: "string",
  },
  {
    id: "subTotalApproved",
    label: "Aprovado",
    align: "right",
    type: "string",
    applyColorMonetary: true,
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
  {
    id: "subTotalNotApproved",
    label: "A Aprovar",
    align: "right",
    type: "string",
    applyColorMonetary: true,
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
  {
    id: "subTotalPreApproved",
    label: "Pré-Aprovado",
    align: "right",
    type: "string",
    applyColorMonetary: true,
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
  {
    id: "subTotal",
    label: "Total",
    align: "right",
    type: "string",
    applyColorMonetary: true,
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
];

const BankTransactionList = () => {
  const [isPostBack, setIsPostBack] = useState(false);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [total, setTotal] = useState(0);
  const [data, setData] = useState<any>([]);
  const [dataIn, setDataIn] = useState<any>([]);
  const [dataTotalIn, setdataTotalIn] = useState<any>([]);
  const [dataOut, setDataOut] = useState<any>([]);
  const [dataTotalOut, setDataTotalOut] = useState<any>([]);
  const [dataTotal, setDataTotal] = useState<any>([]);
  const [expanded, setExpanded] = React.useState<true | false>(true);
  const classes = bankTransactionListStyles();
  const cashFlowService = new CashFlowService();
  const [dataCashFlow, setDataCashFlow] = useState<any>();

  const initialSearch = {
    bankAccountId: 0,
    visible: { id: true, description: "Visivel" },
    date: new Date(),
  };

  const totalColumn = columns.length;
  const minWidth = 100 / totalColumn;

  columns.forEach((element) => {
    element.minWidth = minWidth.toString() + "%";
  });

  const [search, setSearch] = useState<any>(initialSearch);

  const getList = (filter: any) => {
    setLoading(true);
    setIsPostBack(true);

    filter = {
      bankAccountId: filter.bankAccount?.id,
      date: filter.date,
      categoryId: filter.category?.id,
      operationId: filter.operation?.id,
      approved: filter.approved?.id,
      inOut: filter.inOut?.id,
      visible: filter.visible?.id,
      includeZeros: filter.include?.id,
    } as CashFlowFilterModel;

    cashFlowService
      .getList(filter)
      .then((response: any) => {
        let index = 0;

        response.forEach((element: any) => {
          element.index = index;
          if (element.isDetailedTransaction) {
            let amount = 0;
            if (element.cashFlowDetaileds.length > 0) {
              element.cashFlowDetaileds?.forEach((subElement: any) => {
                subElement.delete = false;
              });
              amount = element.cashFlowDetaileds?.reduce(
                (sum: number, current: any) =>
                  sum + (current ? current.amount : 0),
                0
              );
            }
            element.amount = amount;
          }
          index++;
        });

        setData(response);

        const totalApprovationNeeded = response.filter(
          (x: any) => x.domain?.approvationNeeded === true
        ).length;

        const totalApprovation = response.filter(
          (x: any) => x.approval === true && x.domain.approvationNeeded === true
        ).length;

        const approvalAll = totalApprovationNeeded === totalApprovation;

        setSelectedAll(approvalAll);

        setTotal(response.length);
        setRowsPerPage(response.length);
        setLoading(false);
      })
      .catch((errors) => {
        console.error(errors);
        setLoading(false);
      });
  };

  const getTotalization = (filter: any) => {
    filter = {
      bankAccountId: filter.bankAccount?.id,
      date: filter.date,
      categoryId: filter.category?.id,
      operationId: filter.operation?.id,
      approved: filter.approved?.id,
      inOut: filter.inOut?.id,
      includeZeros: false,
    } as TransactionFilterModel;

    cashFlowService
      .getTotalization(filter)
      .then((response: any) => {
        let totalin = [];
        totalin.push("TOTAL IN");
        totalin.push(response.subtotalIn);
        setDataIn(response.itemsIn);
        setdataTotalIn(totalin);

        let totalOut = [];
        totalOut.push("TOTAL OUT");
        totalOut.push(response.subtotalApproved);
        totalOut.push(response.subtotalNotApproved);
        totalOut.push(response.subtotalPreApproved);
        totalOut.push(response.subtotalOut);
        setDataOut(response.itemsOut);
        setDataTotalOut(totalOut);

        let total = [];

        total.push("TOTAL NET");
        total.push(response.total);

        setDataTotal(total);
      })
      .catch((errors) => {
        console.error(errors);
        setLoading(false);
      });
  };

  const save = (cashFlowlist: CashFlowModel[]) => {
    setLoading(true);
    setIsPostBack(true);
    cashFlowService
      .save(cashFlowlist)
      .then((data: any) => {
        onSubmit();
        setLoading(false);
        snackBarOpen(
          messageService.success.MSG001.replace("{0}", "Movimentação Bancária"),
          SnackBarTypeEnum.SUCCESS
        );
      })
      .catch((error) => {
        setLoading(false);
        if (error.response) {
          // console.log(error.response.data);
          snackBarOpen(error.response.data, SnackBarTypeEnum.ERROR);
        } else if (error.request) {
          snackBarOpen(
            messageService.error.MSG003.replace("{0}", "Movimentação Bancária"),
            SnackBarTypeEnum.ERROR
          );
        } else {
          // Something happened in setting up the request and triggered an Error
          console.log("Error", error.message);
        }
      });
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

  const handleChange = (event: any, i: number) => {
    const name = event.target.name;
    let value = event.target.value;
    i = i + page * rowsPerPage;
    const newData = data.map((item: any, index: number) => {
      if (index === i) {
        const updatedItem = {
          ...item,
          amount:
            name === "amount" && item.amount !== value
              ? value !== ""
                ? parseFloat(value)
                : 0
              : parseFloat(item.amount),
          approval: name === "approval" ? !item.approval : item.approval,
          isChanged: true,
        };

        return updatedItem;
      }

      return item;
    });

    setData(newData);

    if (name === "approval") {
      const totalApprovationNeeded = data.filter(
        (x: any) => x.domain?.approvationNeeded === true
      ).length;

      const totalApprovation = newData.filter(
        (x: any) => x.approval === true && x.domain.approvationNeeded === true
      ).length;

      const approvalAll = totalApprovationNeeded === totalApprovation;

      setSelectedAll(approvalAll);
    }
  };

  const onSubmit = () => {
    getList(search);
    getTotalization(search);
  };

  const onSaveChange = () => {
    if (data.length !== 0) {
      const items = data.filter((x: any) => x.isChanged === true);

      items.forEach((element: any) => {
        element.date = search.date;
        element.approval =
          element.approval === null || element.approval === undefined
            ? false
            : element.approval;
      });
      save(items);
    }
  };

  const exportService = new ExportService();
  const onExportSubmit = () => {
    exportService
      .getExportReportBanckTransaction(search)
      .catch((errors) => {
        if (errors.response) {
          var enc = new TextDecoder("utf-8");
          var arr = new Uint8Array(errors.response.data);
          var mensagem = enc.decode(arr);
          snackBarOpen(mensagem, SnackBarTypeEnum.ERROR);
        } else if (errors.request) {
          snackBarOpen(
            messageService.error.MSG009.replace("{0}", "Arquivo").replace(
              "{1}",
              "Exportação de Movimentações"
            ),

            SnackBarTypeEnum.ERROR
          );
        } else {
          // Something happened in setting up the request and triggered an Error
          console.log("Error", errors.message);
          snackBarOpen(
            messageService.error.MSG009.replace("{0}", "Arquivo").replace(
              "{1}",
              "Exportação de Movimentações"
            ),
            SnackBarTypeEnum.ERROR
          );
        }
      })
      .finally(() => {});
  };

  const [snackOpen, setSnackOpen] = useState(false);
  const [message, setMessage] = useState("");
  const [titleModal, setTitleModal] = useState("");
  const [typeSnack, setTypeSnack] = useState(SnackBarTypeEnum.INFO);

  const snackBarOpen = (message: string, type: SnackBarTypeEnum) => {
    setSnackOpen(true);
    setMessage(message);
    setTypeSnack(type);
  };

  const [selectedAll, setSelectedAll] = React.useState(false);
  const [confirm, setConfirm] = React.useState(true);

  const handleSelectAllClick = (event: any) => {
    const checked = event.target.checked;
    setConfirm(checked);
    const description = checked
      ? messageService.info.MSG001
      : messageService.info.MSG002;

    handlerModal(description, "Atenção");
  };

  const disabledInterrupt = (cashFlow: CashFlowModel) => {
    if (cashFlow?.domain?.approvationNeeded !== true) {
      return true;
    }
    return false;
  };

  const [openModal, setOpenModal] = useState(false);
  const [typeModal, setTypeModal] = useState<ModalTypeEnum>();

  const handlerModal = (message: string, title: string) => {
    setTypeModal(ModalTypeEnum.CONFIRM);
    setOpenModal(true);
    setMessage(message);
    setTitleModal(title);
  };

  const handlerDeniedModal = () => {
    setSelectedAll(!confirm);
    setOpenModal(false);
  };

  const handleClickConfirmModal = () => {
    setSelectedAll(confirm);
    setOpenModal(false);
    const newData = data.map((item: any) => {
      if (item.domain.approvationNeeded === true) {
        const updatedItem = {
          ...item,
          approval: confirm,
          isChanged: true,
        };
        return updatedItem;
      }
      return item;
    });

    setData(newData);
  };

  const handleAccordionChange = () => {
    //const expandedChanded = expanded;
    setExpanded(!expanded);
  };

  const [openDetail, setOpenDetail] = useState(false);

  const onDetail = (cashFlow?: CashFlowModel) => {
    setOpenDetail(true);
    setDataCashFlow(cashFlow);
  };

  useEffect(() => {
    let amount = dataCashFlow?.cashFlowDetaileds
      ?.filter((x: any) => !x.delete)
      .reduce(
        (sum: number, current: any) => sum + (current ? current.amount : 0),
        0
      );

    const dataNew = data.map((item: any, index: number) => {
      if (dataCashFlow?.index === item.index) {
        const updatedItem = {
          ...item,
          amount: amount,
          cashFlowDetaileds: dataCashFlow.cashFlowDetaileds,
        };
        return updatedItem;
      }
      return item;
    });

    setData(dataNew);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [dataCashFlow]);

  useEffect(() => {
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <>
      <div className={classes.root}>
        <BankTransactionTableHeader
          onSubmit={onSubmit}
          search={search}
          setSearch={setSearch}
        />
        {(() => {
          if (loading) {
            return <Spinner />;
          }
          if (isPostBack) {
            if (data.length === 0) {
              return (
                <Paper>
                  {data.length === 0 && (
                    <TableContainer className={classes.containertable}>
                      <Table stickyHeader aria-label="sticky table">
                        <TableBody>
                          <TableRow>
                            <TableCell align="center" colSpan={columns.length}>
                              Resultado não encontrado.
                            </TableCell>
                          </TableRow>
                        </TableBody>
                      </Table>
                    </TableContainer>
                  )}
                </Paper>
              );
            } else {
              return (
                <>
                  <Accordion
                    expanded={expanded}
                    onChange={handleAccordionChange}
                    className={classes.root}
                  >
                    <AccordionSummary
                      expandIcon={<ExpandMoreIcon />}
                      aria-controls="panel1a-content"
                      id="panel1a-header"
                    >
                      <Typography
                        gutterBottom
                        color="secondary"
                        variant="h5"
                        component="h2"
                      >
                        Lista de Movimentações
                      </Typography>
                    </AccordionSummary>
                    <AccordionDetails>
                      <div className={classes.root}>
                        <Grid
                          container
                          md={12}
                          alignItems="center"
                          justify="space-between"
                        >
                          <Grid item md={11}></Grid>
                          <Grid item md={1}>
                            <Tooltip
                              title="Salvar Movimentações"
                              aria-label="add"
                            >
                              <Button
                                disableElevation
                                variant="contained"
                                color="primary"
                                onClick={() => onSaveChange()}
                              >
                                <Icon className={classes.icon}>save</Icon>
                              </Button>
                            </Tooltip>
                          </Grid>
                        </Grid>

                        <TableContainer className={classes.containertable}>
                          <Table
                            size={"small"}
                            stickyHeader
                            aria-label="sticky table"
                          >
                            <TableHead>
                              <TableRow>
                                {columns.map((column: any, index: any) => {
                                  if (column.type === "checkbox") {
                                    return (
                                      <TableCell
                                        className={classes.head}
                                        key={column.id}
                                        align={column.align}
                                        style={{ minWidth: column.minWidth }}
                                      >
                                        {"Aprovado"}
                                        <FormControl
                                          component="fieldset"
                                          style={{
                                            display: "flex",
                                            alignItems: "center",
                                          }}
                                        >
                                          <FormGroup>
                                            <FormControlLabel
                                              value={selectedAll}
                                              checked={selectedAll}
                                              onChange={(e) =>
                                                handleSelectAllClick(e)
                                              }
                                              name="approvalAll"
                                              control={
                                                <Checkbox
                                                  size="small"
                                                  className={classes.head}
                                                />
                                              }
                                              label="Todos"
                                              labelPlacement="end"
                                            />
                                          </FormGroup>
                                        </FormControl>
                                      </TableCell>
                                    );
                                  } else if (column.type === "button") {
                                    return (
                                      <TableCell
                                        className={classes.head}
                                        key={column.id}
                                        align={column.align}
                                        style={{
                                          minWidth: column.minWidth,
                                        }}
                                      >
                                        {column.label}
                                      </TableCell>
                                    );
                                  } else {
                                    return (
                                      <TableCell
                                        className={classes.head}
                                        key={column.id}
                                        align={column.align}
                                        style={{ minWidth: column.minWidth }}
                                      >
                                        {column.label}
                                      </TableCell>
                                    );
                                  }
                                })}
                              </TableRow>
                            </TableHead>
                            <TableBody>
                              {data
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
                                        if (
                                          data.length > 0 &&
                                          names.length === 2
                                        ) {
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

                                        if (column.type === "checkbox") {
                                          return (
                                            <TableCell
                                              className={classes.cellText}
                                              key={column.id}
                                              align={column.align}
                                            >
                                              <FormGroup>
                                                <Typography
                                                  align={column.align}
                                                  component="div"
                                                >
                                                  {!disabledInterrupt(row) && (
                                                    <Checkbox
                                                      disabled={disabledInterrupt(
                                                        row
                                                      )}
                                                      checked={value}
                                                      size="small"
                                                      name="approval"
                                                      onChange={(e) =>
                                                        handleChange(e, index)
                                                      }
                                                      inputProps={{
                                                        "aria-label":
                                                          "primary checkbox",
                                                      }}
                                                    />
                                                  )}
                                                </Typography>
                                              </FormGroup>
                                            </TableCell>
                                          );
                                        } else if (
                                          column.type === "textfield"
                                        ) {
                                          return (
                                            <TableCell
                                              className={classes.cellText}
                                              key={column.id}
                                              align={column.align}
                                            >
                                              <TextField
                                                //className={classes.textfield}
                                                type="text"
                                                name="amount"
                                                size="small"
                                                value={
                                                  column.format
                                                    ? column.format(value)
                                                    : value
                                                }
                                                onChange={(e) =>
                                                  handleChange(e, index)
                                                }
                                                variant="outlined"
                                                inputProps={{
                                                  min: 0,
                                                  //maxLength: "15",
                                                  style: { textAlign: "end" },
                                                }}
                                                InputProps={{
                                                  inputComponent:
                                                    NumberFormatCustomWithOutNegative as any,
                                                  readOnly:
                                                    row.domain
                                                      .isDetailedTransaction,
                                                }}
                                              />
                                            </TableCell>
                                          );
                                        } else if (column.type === "button") {
                                          return (
                                            <TableCell
                                              className={classes.cellText}
                                              key={column.id}
                                              align={column.align}
                                            >
                                              {value && (
                                                <Tooltip
                                                  title="Adicionar detalhes"
                                                  aria-label="add"
                                                >
                                                  <IconButton
                                                    onClick={() =>
                                                      onDetail(row)
                                                    }
                                                    aria-label="delete"
                                                  >
                                                    <Icon>post_add</Icon>
                                                  </IconButton>
                                                </Tooltip>
                                              )}
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
                          ActionsComponent={EcashTablePaginationActions}
                        />
                        <Grid
                          container
                          spacing={2}
                          alignItems="center"
                          justify="space-between"
                          className={classes.paddingSave}
                        >
                          <Grid item md={6}></Grid>
                          <Grid item md={2}>
                            <Button
                              disableElevation
                              fullWidth
                              variant="contained"
                              color="secondary"
                              classes={{ label: classes.button }}
                              onClick={() => onSubmit()}
                            >
                              <Icon className={classes.icon}>clear</Icon>
                              Limpar
                            </Button>
                          </Grid>
                          <Grid container item md={2}>
                            <Button
                              disableElevation
                              fullWidth
                              variant="contained"
                              color="secondary"
                              classes={{ label: classes.button }}
                              onClick={() => onExportSubmit()}
                            >
                              <Icon className={classes.icon}>publish</Icon>
                              Exportar
                            </Button>
                          </Grid>
                          <Grid item md={2}>
                            <Button
                              disableElevation
                              fullWidth
                              variant="contained"
                              color="primary"
                              classes={{ label: classes.button }}
                              onClick={() => onSaveChange()}
                            >
                              <Icon className={classes.icon}>save</Icon>
                              Salvar
                            </Button>
                          </Grid>
                        </Grid>
                      </div>
                    </AccordionDetails>
                  </Accordion>
                  <Accordion>
                    <AccordionSummary
                      expandIcon={<ExpandMoreIcon />}
                      aria-controls="panel2a-content"
                      id="panel2a-header"
                    >
                      <Typography
                        gutterBottom
                        color="secondary"
                        variant="h5"
                        component="h2"
                      >
                        Totalizadores
                      </Typography>
                    </AccordionSummary>
                    <AccordionDetails>
                      <div className={classes.root}>
                        <Grid
                          container
                          spacing={2}
                          alignItems="flex-start"
                          justify="space-between"
                        >
                          <Grid item md={4}>
                            <TotalizationList
                              titleTable={"ENTRADAS"}
                              data={dataIn}
                              columns={columnsIn}
                              //totalData={dataTotalIn}
                            />
                          </Grid>
                          <Grid item md={8}>
                            <TotalizationList
                              titleTable={"SAÍDAS"}
                              data={dataOut}
                              columns={columnsOut}
                              //totalData={dataTotalOut}
                            />
                          </Grid>

                          <Grid
                            container
                            alignItems="flex-end"
                            justify="space-between"
                            spacing={2}
                            item
                          >
                            <Grid item md={4}>
                              <TableContainer
                                className={classes.containertable}
                              >
                                <Table
                                  size={"small"}
                                  stickyHeader
                                  aria-label="sticky table"
                                >
                                  <TableHead>
                                    <TableRow>
                                      {dataTotalIn.map(
                                        (value: any, index: number) => {
                                          if (validation.isNumber(value)) {
                                            const classeText =
                                              parseFloat(value) < 0
                                                ? classes.cellTextRed
                                                : classes.cellTextBlue;
                                            return (
                                              <TableCell
                                                className={classeText}
                                                key={index}
                                                align={"right"}
                                                style={{
                                                  width:
                                                    (
                                                      100 / columnsIn.length
                                                    ).toString() + "%",
                                                }}
                                              >
                                                {value.toLocaleString("pt-BR", {
                                                  minimumFractionDigits: 2,
                                                  maximumFractionDigits: 2,
                                                })}
                                              </TableCell>
                                            );
                                          } else {
                                            return (
                                              <TableCell
                                                className={classes.head}
                                                key={index}
                                                align={"left"}
                                                style={{
                                                  width:
                                                    (
                                                      100 / columnsIn.length
                                                    ).toString() + "%",
                                                }}
                                              >
                                                {value}
                                              </TableCell>
                                            );
                                          }
                                        }
                                      )}
                                    </TableRow>
                                  </TableHead>
                                </Table>
                              </TableContainer>
                            </Grid>
                            <Grid item md={8}>
                              <TableContainer
                                className={classes.containertable}
                              >
                                <Table
                                  size={"small"}
                                  stickyHeader
                                  aria-label="sticky table"
                                >
                                  <TableHead>
                                    <TableRow>
                                      {dataTotalOut.map(
                                        (value: any, index: number) => {
                                          if (validation.isNumber(value)) {
                                            const classeText =
                                              parseFloat(value) < 0
                                                ? classes.cellTextRed
                                                : classes.cellTextBlue;
                                            return (
                                              <TableCell
                                                className={classeText}
                                                key={index}
                                                align={"right"}
                                                style={{
                                                  width:
                                                    (
                                                      100 / columnsOut.length
                                                    ).toString() + "%",
                                                }}
                                              >
                                                {value.toLocaleString("pt-BR", {
                                                  minimumFractionDigits: 2,
                                                  maximumFractionDigits: 2,
                                                })}
                                              </TableCell>
                                            );
                                          } else {
                                            return (
                                              <TableCell
                                                className={classes.head}
                                                key={index}
                                                align={"left"}
                                                style={{
                                                  width:
                                                    (
                                                      100 / columnsOut.length
                                                    ).toString() + "%",
                                                }}
                                              >
                                                {value}
                                              </TableCell>
                                            );
                                          }
                                        }
                                      )}
                                    </TableRow>
                                  </TableHead>
                                </Table>
                              </TableContainer>
                            </Grid>
                          </Grid>
                          <Grid item md={12}>
                            <TableContainer className={classes.containertable}>
                              <Table
                                size={"small"}
                                stickyHeader
                                aria-label="sticky table"
                              >
                                <TableHead>
                                  <TableRow>
                                    {dataTotal.map(
                                      (value: any, index: number) => {
                                        if (validation.isNumber(value)) {
                                          const classeText =
                                            parseFloat(value) < 0
                                              ? classes.cellTextRed
                                              : classes.cellTextBlue;
                                          return (
                                            <TableCell
                                              className={classeText}
                                              key={index}
                                              align={"right"}
                                              style={{
                                                width:
                                                  (
                                                    100 / columnsIn.length
                                                  ).toString() + "%",
                                              }}
                                            >
                                              {value.toLocaleString("pt-BR", {
                                                minimumFractionDigits: 2,
                                                maximumFractionDigits: 2,
                                              })}
                                            </TableCell>
                                          );
                                        } else {
                                          return (
                                            <TableCell
                                              className={classes.head}
                                              key={index}
                                              align={"left"}
                                              style={{
                                                width:
                                                  (
                                                    100 / columnsIn.length
                                                  ).toString() + "%",
                                              }}
                                            >
                                              {value}
                                            </TableCell>
                                          );
                                        }
                                      }
                                    )}
                                  </TableRow>
                                </TableHead>
                              </Table>
                            </TableContainer>
                          </Grid>
                        </Grid>
                      </div>
                    </AccordionDetails>
                  </Accordion>
                </>
              );
            }
          }
        })()}
      </div>
      <EcashSnackbar
        open={snackOpen}
        setOpen={setSnackOpen}
        type={typeSnack}
        message={message}
      />
      <ReportManualPaymentsModal
        open={openDetail}
        setOpen={setOpenDetail}
        dataCashFlow={dataCashFlow}
        setDataCashFlow={setDataCashFlow}
        data={data}
        setData={setData}
      />
      <ActionModal
        open={openModal}
        typeModal={typeModal}
        handleClickStateModal={handlerDeniedModal}
        handleClickConfirmModal={handleClickConfirmModal}
        messageModal={message}
        titleModal={titleModal}
      />
    </>
  );
};

export default BankTransactionList;
