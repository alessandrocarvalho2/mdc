import {
  Grid,
  Button,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  IconButton,
  Dialog,
  TextField,
} from "@material-ui/core";
import { Close } from "@material-ui/icons";
import moment from "moment";
import React, { useEffect, useState } from "react";
import SnackBarTypeEnum from "../../../../../../core/enum/snackbar-type.enum";
import CashAdjustmentModel from "../../../../../../core/models/cash-adjustment.model";
import CashConciliationModel from "../../../../../../core/models/cash-conciliation.model";
import CategoryModel from "../../../../../../core/models/category.model";
import OperationModel from "../../../../../../core/models/operation.model";
import CashFlowService from "../../../../../../core/services/cash-flow.service";
import CashConciliationService from "../../../../../../core/services/cash-conciliation.service";
import messageService from "../../../../../../core/services/message.service";
import BankAccountSelect from "../../../../../../shared/components/ecash-bank-account-select/bank-account.component";
import CategorySelect from "../../../../../../shared/components/ecash-category-select/category.component";
import DatePicker from "../../../../../../shared/components/ecash-date-picker/date-picker.component";
import NumberFormatCustom from "../../../../../../shared/components/ecash-number-format/number-format-custom.component";
import OperationSelect from "../../../../../../shared/components/ecash-operation-select/operation.component";
import EcashSnackbar from "../../../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import Spinner from "../../../../../../shared/components/ecash-spinner/ecash-spinner.component";
import bankTransactionAddModelStyle from "./bank-transaction-add-modal.style";

interface BankTransactionAddModelInterface {
  open: any;
  setOpen: any;
  setAddTransaction: any;
  addTransaction: any;
  dataStatement: any;
  dataCash: any;
  onSubmit: any;
}

const BankTransactionAddModel = (props: BankTransactionAddModelInterface) => {
  const {
    addTransaction,
    setAddTransaction,
    open,
    setOpen,
    dataCash,
    dataStatement,
    onSubmit,
  } = props;

  const classes = bankTransactionAddModelStyle();
  const [loading, setLoading] = useState(false);
  const cashFlowService = new CashFlowService();
  const cashConciliationService = new CashConciliationService();
  const onClose = () => {
    setOpen(false);
  };

  const handlerTextChange = (event: any) => {
    const value = event.target.value;
    setAddTransaction({
      ...addTransaction,
      description: value,
    });
  };

  const handlerdDate = (dateStatement: Date | null) => {
    setAddTransaction({
      ...addTransaction,
      date: dateStatement,
    });
  };

  const onSave = (e: any) => {
    e.preventDefault();

    if (validate()) {
      //setDisabled(true);
      setLoading(true);
      const cashAdjustment = {
        bankAccountId: addTransaction.bankAccount.id,
        categoryId: addTransaction.category?.id,
        operationId: addTransaction.operation?.id,
        inOut: addTransaction.inOut,
        description: addTransaction.description,
        date: addTransaction.date,
        amount: parseFloat(addTransaction.balance),
      } as CashAdjustmentModel;

      cashFlowService
        .saveAdjustment(cashAdjustment)
        .then((resp: any) => {
          let data = dataCash;
          resp.checkedCash = true;
          data.push(resp);

          const item = {
            transactions: dataStatement.filter((x: any) => x.checkedStatement),
            cashFlows: data.filter((x: any) => x.checkedCash),
            date: addTransaction.date,
          } as CashConciliationModel;

          onSaveConciliation(item);
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
        .finally(() => {
          //setLoading(false);
        });
    }
  };

  const onSaveConciliation = (item: CashConciliationModel) => {
    cashConciliationService
      .save(item)
      .then((resp: any) => {
        setOpen(false);
        onSubmit();
        snackBarOpen(
          messageService.success.MSG003.replace("{0}", "Conciliação"),
          SnackBarTypeEnum.SUCCESS
        );
      })
      .catch((error) => {
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
      .finally(() => {
        setLoading(false);
      });
  };

  const initialFValues = {
    date: "",
    bankAccount: "",
    category: "",
    operation: "",
    balance: "",
    description: "",
  };

  const [errors, setErrors] = useState(initialFValues);

  const validate = () => {
    let temp = { ...errors };
    const formatOk = /^(\d{2})\/(\d{2})\/(\d{4})$/.test(
      moment(addTransaction?.date, "DD/MM/YYYY").format("DD/MM/YYYY")
    );

    let isValid = true;
    if (
      addTransaction?.date === undefined ||
      addTransaction?.date === null ||
      addTransaction?.date === ""
    ) {
      temp.date = "Informe um data.";
      isValid = true;
    } else if (addTransaction?.date === "Invalid Date" || !formatOk) {
      temp.date = "Data inválida";
      isValid = true;
    }

    if (
      addTransaction?.bankAccount?.id === "" ||
      addTransaction?.bankAccount?.id === undefined ||
      addTransaction?.bankAccount?.id === null ||
      addTransaction?.bankAccount?.id === 0
    ) {
      temp.bankAccount = "Selecione uma Conta.";
      isValid = false;
    }

    if (
      addTransaction?.category?.id === "" ||
      addTransaction?.category?.id === undefined ||
      addTransaction?.category?.id === null ||
      addTransaction?.category?.id === 0
    ) {
      temp.category = "Selecione uma Categoria.";
      isValid = false;
    }

    if (
      addTransaction?.operation?.id === "" ||
      addTransaction?.operation?.id === undefined ||
      addTransaction?.operation?.id === null ||
      addTransaction?.operation?.id === 0
    ) {
      temp.operation = "Selecione uma Operação.";
      isValid = false;
    }

    if (
      addTransaction?.description === "" ||
      addTransaction?.description === undefined ||
      addTransaction?.description === null
    ) {
      temp.description = "Informe a descrição.";
      isValid = false;
    }
    setErrors({
      ...temp,
    });

    return isValid;
  };

  const clearErrors = () => {
    let temp = { ...errors };

    if (addTransaction?.bankAccount > 0) {
      temp.bankAccount = "";
      setErrors({
        ...temp,
      });
    }

    if (addTransaction?.category?.id > 0) {
      temp.category = "";
      setErrors({
        ...temp,
      });
    }

    if (addTransaction?.operation?.id > 0) {
      temp.operation = "";
      setErrors({
        ...temp,
      });
    }

    if (addTransaction?.date && addTransaction?.date !== "Invalid Date") {
      temp.date = "";
      setErrors({
        ...temp,
      });
    }

    if (
      addTransaction?.balance !== undefined &&
      addTransaction?.balance !== "" &&
      parseFloat(addTransaction?.balance) !== 0
    ) {
      temp.balance = "";
      setErrors({
        ...temp,
      });
    }

    if (
      addTransaction?.description !== undefined &&
      addTransaction?.description !== "" &&
      addTransaction?.description !== null
    ) {
      temp.description = "";
      setErrors({
        ...temp,
      });
    }
  };

  useEffect(() => {
    clearErrors();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    addTransaction?.bankAccount,
    addTransaction?.category,
    addTransaction?.operation,
    addTransaction?.description,
    addTransaction?.date,
    addTransaction?.balance,
  ]);

  const [snackOpen, setSnackOpen] = useState(false);
  const [message, setMessage] = useState("");
  const [typeSnack, setTypeSnack] = useState(SnackBarTypeEnum.INFO);

  const snackBarOpen = (message: string, type: SnackBarTypeEnum) => {
    setSnackOpen(true);
    setMessage(message);
    setTypeSnack(type);
  };

  useEffect(() => {
    setFilterDefault({
      bankAccountId: addTransaction.bankAccount?.id,
      inOut: addTransaction.inOut,
      categoryId: addTransaction.category?.id,
    });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [addTransaction]);

  useEffect(() => {
    setAddTransaction({
      ...addTransaction,
      bankAccount: addTransaction.bankAccount,
      description: addTransaction.description,
      inOut: addTransaction.inOut,
      category: addTransaction.category,
      operation: undefined,
    });

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [addTransaction.category?.id]);

  const handlerChangeSelect = (name: any, value: any) => {
    setAddTransaction({
      ...addTransaction,
      [name]:
        value !== undefined && value !== null && value !== "" ? value : "",
    });
  };

  const initialValueCategory: CategoryModel = {
    id: undefined,
    description: "Selecione",
    bankAccountId: undefined,
    inOut: undefined,
  };

  const initialValueOperation: OperationModel = {
    id: undefined,
    code: "",
    description: "Selecione",
  };

  const [filterDefault, setFilterDefault] = useState<any>({});

  return (
    <>
      {(() => {
        if (loading) {
          return <Spinner />;
        }
        return (
          <div>
            <Dialog
              open={open}
              onClose={() => onClose()}
              aria-labelledby="responsive-dialog-title"
              classes={{
                paper: classes.modal,
              }}
            >
              <div className={classes.closeButton}>
                <IconButton
                  size="small"
                  aria-label="delete"
                  onClick={() => onClose()}
                >
                  <Close />
                </IconButton>
              </div>
              <DialogTitle
                id="responsive-dialog-title"
                classes={{
                  root: classes.dialogTitle,
                }}
              >
                Adicionar Distorção de Caixa
              </DialogTitle>
              <DialogContent>
                <DialogContentText classes={{ root: classes.text }}>
                  {" "}
                </DialogContentText>
                <Grid
                  container
                  item
                  spacing={2}
                  alignItems="center"
                  justify="space-between"
                >
                  <Grid item md={6}>
                    <DatePicker
                      name="date"
                      error={errors.date ? true : false}
                      helperText={errors.date}
                      setSelectedDate={handlerdDate}
                      date={addTransaction?.date}
                      label="Data do MovimeWntação"
                      disabled={true}
                      disableFuture={true}
                      disablePast={true}
                    />
                  </Grid>
                  <Grid item md={6}>
                    <BankAccountSelect
                      disabled={true}
                      initialValue={addTransaction?.bankAccount}
                      error={errors.bankAccount ? true : false}
                      helperText={errors.bankAccount}
                      handlerChangeSelect={handlerChangeSelect}
                    />
                  </Grid>

                  <Grid item md={6}>
                    <CategorySelect
                      value={addTransaction.category}
                      error={errors.category ? true : false}
                      helperText={errors.category}
                      filter={filterDefault}
                      initialValue={initialValueCategory}
                      handlerChangeSelect={handlerChangeSelect}
                    />
                  </Grid>

                  <Grid item md={6}>
                    <OperationSelect
                      disabled={addTransaction.category?.id ? false : true}
                      value={addTransaction.operation}
                      //setValue={setAddTransaction}
                      initialValue={initialValueOperation}
                      filter={filterDefault}
                      error={errors.operation ? true : false}
                      helperText={errors.operation}
                      handlerChangeSelect={handlerChangeSelect}
                    />
                  </Grid>

                  <Grid item md={6}>
                    <TextField
                      type="text"
                      value={addTransaction?.description}
                      error={errors.description ? true : false}
                      helperText={errors.description}
                      name="description"
                      size="small"
                      label="Descrição:"
                      onChange={handlerTextChange}
                      variant="standard"
                      placeholder="Informe a descrição."
                    />
                  </Grid>
                  <Grid item md={6}>
                    <TextField
                      type="text"
                      value={addTransaction?.balance}
                      error={errors.balance ? true : false}
                      helperText={errors.balance}
                      name="balance"
                      size="small"
                      label="Valor:"
                      onChange={handlerTextChange}
                      variant="standard"
                      placeholder="Informe a Valor."
                      inputProps={{
                        min: 0,
                        style: { textAlign: "end" },
                      }}
                      InputProps={{
                        inputComponent: NumberFormatCustom as any,
                        readOnly: true,
                      }}
                    />
                  </Grid>
                </Grid>
              </DialogContent>
              <DialogActions className={classes.actions}>
                <Button
                  autoFocus
                  variant="outlined"
                  onClick={() => onClose()}
                  color="secondary"
                >
                  Cancel
                </Button>
                <Button
                  onClick={(e) => {
                    onSave(e);
                  }}
                  variant="contained"
                  color="primary"
                  classes={{ root: classes.button }}
                >
                  Incluir
                </Button>
              </DialogActions>
            </Dialog>

            <EcashSnackbar
              open={snackOpen}
              setOpen={setSnackOpen}
              type={typeSnack}
              message={message}
            />
          </div>
        );
      })()}
    </>
  );
};

export default BankTransactionAddModel;
