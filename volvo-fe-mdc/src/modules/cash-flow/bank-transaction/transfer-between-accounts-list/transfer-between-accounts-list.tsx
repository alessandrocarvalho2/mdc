import {
  Button,
  Grid,
  Icon,
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
import ModalTypeEnum from "../../../../core/enum/modal-type.enum";
import SnackBarTypeEnum from "../../../../core/enum/snackbar-type.enum";
import messageService from "../../../../core/services/message.service";
import ReportService from "../../../../core/services/report.service";
import ActionModal from "../../../../shared/components/ecash-action-modal/action-modal.component";
import EcashSnackbar from "../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import Spinner from "../../../../shared/components/ecash-spinner/ecash-spinner.component";
import transferBetweenAccountsListStyles from "./transfer-between-accounts-list.style";

interface Column {
  id:
    | "id"
    | "originAccount.bank.bankCode"
    | "originAccount.nickname"
    | "originAccount.agency"
    | "originAccount.account"
    | "amountToTransfer"
    | "destinyAccount.bank.bankCode"
    | "destinyAccount.nickname"
    | "destinyAccount.agency"
    | "destinyAccount.account";
  label: string;
  width?: string;
  align?: "left" | "center" | "right";
  type?: "string" | "textfield" | "checkbox";
  format?: (value: any) => string;
}

let columns: Column[] = [
  {
    id: "originAccount.bank.bankCode",
    label: "ID",
    align: "left",
    type: "string",
    width: "5%",
  },
  {
    id: "originAccount.nickname",
    label: "Banco",
    align: "left",
    type: "string",
    width: "12,8571%",
  },
  {
    id: "originAccount.agency",
    label: "Agência",
    align: "left",
    type: "string",
    width: "12,8571%",
  },
  {
    id: "originAccount.account",
    label: "Conta",
    align: "left",
    type: "string",
    width: "12,8571%",
  },
  {
    id: "amountToTransfer",
    label: "Valor a Transferir",
    align: "right",
    type: "string",
    width: "12,8571%",
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
  {
    id: "destinyAccount.bank.bankCode",
    label: "ID",
    align: "left",
    type: "string",
    width: "5%",
  },
  {
    id: "destinyAccount.nickname",
    label: "Banco",
    align: "left",
    type: "string",
    width: "12,8571%",
  },
  {
    id: "destinyAccount.agency",
    label: "Agência",
    align: "left",
    type: "string",
    width: "12,8571%",
  },
  {
    id: "destinyAccount.account",
    label: "Conta",
    align: "left",
    type: "string",
    width: "12,8571%",
  },
];

const TransferBetweenAccountsList = () => {
  const [isPostBack, setIsPostBack] = useState(false);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [total, setTotal] = useState(0);
  const [data, setData] = useState<any>();

  const classes = transferBetweenAccountsListStyles();
  const reportService = new ReportService();

  const getList = () => {
    setLoading(true);
    reportService
      .getCashTransferReport(new Date())
      .then((response: any) => {
        console.log(response);
        const transferBetweenAccountItems = response;
        setData(transferBetweenAccountItems);
        setTotal(transferBetweenAccountItems.length);
        setRowsPerPage(transferBetweenAccountItems.length);
        setIsPostBack(true);
        setLoading(false);
      })
      .catch((errors) => {
        console.error(errors);
        setIsPostBack(false);
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

  const [snackOpen, setSnackOpen] = useState(false);
  const [message, setMessage] = useState("");
  const [typeSnack, setTypeSnack] = useState(SnackBarTypeEnum.INFO);

  const snackBarOpen = (message: string, type: SnackBarTypeEnum) => {
    setSnackOpen(true);
    setMessage(message);
    setTypeSnack(type);
  };

  const getColorCell = (field: string, row: any) => {
    if (field === "originAccount" && row?.originAccount?.isMainAccount) {
      return classes.backgroundColorRowMain;
    }
    if (field === "destinyAccount" && row?.destinyAccount?.isMainAccount) {
      return classes.backgroundColorRowMain;
    }

    return "";
  };

  const onApplyTransfer = () => {
    save();
  };

  const save = () => {
    setLoading(true);
    setIsPostBack(true);
    reportService
      .saveCashTransferReport(new Date())
      .then((data: any) => {
        setLoading(false);
        console.log(messageService.success.MSG003.replace("{0}", "Transferências entre Contas"))  ;

        snackBarOpen(
          messageService.success.MSG003.replace("{0}", "Transferências entre Contas"),
          SnackBarTypeEnum.SUCCESS
        );
      })
      .catch((error) => {
        setLoading(false);
        if (error.response) {
          // console.log(error.response.data);
          if (error.response.data !== "") {
            snackBarOpen(error.response.data.toString(), SnackBarTypeEnum.ERROR);
          }else if (error.request) {
            snackBarOpen(
              messageService.error.MSG004.replace("{0}", "Transferências entre Contas"),
              SnackBarTypeEnum.ERROR
            );
          }
        } else if (error.request) {
          snackBarOpen(
            messageService.error.MSG004.replace("{0}", "Transferências entre Contas"),
            SnackBarTypeEnum.ERROR
          );
        } else {
          // Something happened in setting up the request and triggered an Error
          console.log("Error", error.message);
        }
      });
  };

  const [openModal, setOpenModal] = useState(false);
  const [typeModal, setTypeModal] = useState<ModalTypeEnum>();
  const [titleModal, setTitleModal] = useState("");

  const handlerModal = () => {
    setTypeModal(ModalTypeEnum.CONFIRM);
    setOpenModal(true);
    setMessage(
      messageService.info.MSG003,
    );
    setTitleModal("Atenção");
  };

  const handlerDeniedModal = () => {
    setOpenModal(false);
  };

  const handleClickConfirmModal = () => {
    setOpenModal(false);
    onApplyTransfer();
  };

  return (
    <>
      <div className={classes.root}>
        <Grid container spacing={4} alignItems="center" justify="space-between">
          <Grid item md={8}>
            <span className={classes.title}>Transferências entre Contas</span>
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
                    <Table stickyHeader aria-label="sticky table">
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
                    <Table
                      stickyHeader
                      size={"small"}
                      aria-label="sticky table"
                    >
                      <TableHead>
                        <TableRow>
                          <TableCell
                            colSpan={4}
                            style={{ width: "43,57145%" }}
                            align="center"
                            className={classes.head}
                          >
                            {"CONTA ORIGEM "}
                          </TableCell>
                          <TableCell
                            style={{ width: "12,8571%" }}
                            className={classes.head}
                          >
                            {" "}
                          </TableCell>
                          <TableCell
                            colSpan={4}
                            align="center"
                            style={{ width: "43,57145%" }}
                            className={classes.head}
                          >
                            {"CONTA DESTINO "}
                          </TableCell>
                        </TableRow>

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
                                // className={
                                //   !row?.originAccount?.isMainAccount
                                //     ? classes.backgroundColorRowMain
                                //     : ""
                                // }
                              >
                                {columns.map((column) => {
                                  let value = row[column.id];
                                  let names = column.id.split(".");

                                  let classeText = "";
                                  if (data.length > 0 && names.length === 2) {
                                    classeText = getColorCell(names[0], row);

                                    const obj = row[names[0]][names[1]];
                                    value = obj;
                                  } else if (
                                    data.length > 0 &&
                                    names.length === 3
                                  ) {
                                    classeText = getColorCell(names[0], row);
                                    const obj =
                                      row[names[0]][names[1]][names[2]];
                                    value = obj;
                                  }

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
                  <Grid
                    container
                    spacing={2}
                    alignItems="center"
                    justify="space-between"
                    className={classes.paddingSave}
                  >
                    <Grid item md={9}></Grid>
                    <Grid item md={3}>
                      <Button
                        disableElevation
                        fullWidth
                        variant="contained"
                        color="primary"
                        classes={{ label: classes.button }}
                        onClick={() => handlerModal()}
                      >
                        <Icon className={classes.icon}>swap_horiz_icon</Icon>
                        Aplicar Tranferência
                      </Button>
                    </Grid>
                  </Grid>
                </Paper>
              );
            }
          }
        })()}
        <EcashSnackbar
          open={snackOpen}
          setOpen={setSnackOpen}
          type={typeSnack}
          message={message}
        />
        <ActionModal
          open={openModal}
          typeModal={typeModal}
          handleClickStateModal={handlerDeniedModal}
          handleClickConfirmModal={handleClickConfirmModal}
          messageModal={message}
          titleModal={titleModal}
        />
      </div>
    </>
  );
};

export default TransferBetweenAccountsList;
