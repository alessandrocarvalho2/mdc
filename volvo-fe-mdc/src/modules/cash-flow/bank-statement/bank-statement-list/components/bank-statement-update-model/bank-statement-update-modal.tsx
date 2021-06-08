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
import AccountBalanceModel from "../../../../../../core/models/account-balance.model copy";
import AccountBalanceService from "../../../../../../core/services/account-balance.service";
import messageService from "../../../../../../core/services/message.service";
import BankAccountSelect from "../../../../../../shared/components/ecash-bank-account-select/bank-account.component";
import DatePicker from "../../../../../../shared/components/ecash-date-picker/date-picker.component";
import NumberFormatCustom from "../../../../../../shared/components/ecash-number-format/number-format-custom.component";
import EcashSnackbar from "../../../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import Spinner from "../../../../../../shared/components/ecash-spinner/ecash-spinner.component";
import bankStatementUpdateModelStyle from "./bank-statement-update-modal.style";
interface BankStatementRemoveActionInterface {
  setOpen: any;
  setSearch: any;
  search: any;
  open: any;
}

const BankStatementUpdatetModal = (
  props: BankStatementRemoveActionInterface
) => {
  const { setSearch, open, setOpen } = props;

  const classes = bankStatementUpdateModelStyle();
  const [loading, setLoading] = useState(false);
  const accountBalanceService = new AccountBalanceService();

  const onClose = () => {
    setOpen(false);
    setSearch({});
    props.setSearch({});
    props.setSearch({ date: new Date() });
  };

  const handlerChange = (name: any, value: any) => {
    props.setSearch({
      ...props.search,
      [name]: value && value.id ? value.id : "",
    });
  };

  const handlerTextChange = (event: any) => {
    const value = event.target.value;
    props.setSearch({
      ...props.search,
      balance: value,
    });
  };

  const handlerdDate = (dateStatement: Date | null) => {
    props.setSearch({
      ...props.search,
      date: dateStatement,
    });
  };

  const onSubmit = (e: any) => {
    e.preventDefault();
    console.log(props.search);

    if (validate()) {
      setLoading(true);
      const accountBalance = {
        bankAccountId: props.search.bankAccount,
        date: props.search.date,
        balance: parseFloat(props.search.balance),
      } as AccountBalanceModel;
      accountBalanceService
        .save(accountBalance)
        .then((resp: any) => {
          console.log(resp);
          setLoading(false);
          snackBarOpen(
            messageService.success.MSG005.replace("{0}", "Saldo Inicial"),
            SnackBarTypeEnum.SUCCESS
          );
          setOpen(false);
        })
        .catch((errors) => {
          console.log(errors.response.data);

          setLoading(false);
          if (errors.response.data !== "") {
            snackBarOpen(errors.response.data.toString(), SnackBarTypeEnum.ERROR);
          } else if (errors.request) {
            snackBarOpen(
              messageService.error.MSG003.replace("{0}", "Saldo inicial"),
              SnackBarTypeEnum.ERROR
            );
          } else {
            // Something happened in setting up the request and triggered an Error
            console.log("Error", errors.message);
            snackBarOpen(
              messageService.error.MSG003.replace("{0}", "Saldo inicial"),
              SnackBarTypeEnum.ERROR
            );
          }
        });
    }
  };

  const initialFValues = {
    date: "",
    bankAccount: "",
    balance: "",
  };

  const [errors, setErrors] = useState(initialFValues);

  const validate = () => {
    let temp = { ...errors };
    const formatOk = /^(\d{2})\/(\d{2})\/(\d{4})$/.test(
      moment(props.search?.date, "DD/MM/YYYY").format("DD/MM/YYYY")
    );

    let isValid = true;
    if (
      props.search?.date === undefined ||
      props.search?.date === null ||
      props.search?.date === ""
    ) {
      temp.date = "Informe um data.";
      isValid = true;
    } else if (props.search?.date === "Invalid Date" || !formatOk) {
      temp.date = "Data inválida";
      isValid = true;
    }

    if (
      props.search?.bankAccount === "" ||
      props.search?.bankAccount === undefined ||
      props.search?.bankAccount === null ||
      props.search?.bankAccount === 0
    ) {
      temp.bankAccount = "Selecione uma Conta Corrente.";
      isValid = false;
    }

    if (
      props.search?.balance === "" ||
      props.search?.balance === undefined ||
      props.search?.balance === null
    ) {
      temp.balance = "Informe o Saldo Inicial.";
      isValid = false;
    }
    setErrors({
      ...temp,
    });

    return isValid;
  };

  const clearErrors = () => {
    let temp = { ...errors };
    if (props.search?.bankAccount > 0) {
      temp.bankAccount = "";
      setErrors({
        ...temp,
      });
    }

    if (props.search?.date && props.search?.date !== "Invalid Date") {
      temp.date = "";
      setErrors({
        ...temp,
      });
    }

    if (props.search?.balance !== undefined && props.search?.balance !== "") {
      temp.balance = "";
      setErrors({
        ...temp,
      });
    }
  };

  useEffect(() => {
    clearErrors();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.search?.bankAccount, props.search?.date, props.search?.amount]);

  const [snackOpen, setSnackOpen] = useState(false);
  const [message, setMessage] = useState("");
  const [typeSnack, setTypeSnack] = useState(SnackBarTypeEnum.INFO);

  const snackBarOpen = (message: string, type: SnackBarTypeEnum) => {
    setSnackOpen(true);
    setMessage(message);
    setTypeSnack(type);
  };

  useEffect(() => {
    props.setSearch({ date: new Date() });
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);
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
                Atualizar Saldo Bancário
              </DialogTitle>
              <DialogContent>
                <DialogContentText classes={{ root: classes.text }}>
                  {" "}
                  Informe a Conta Corrente e o Valor para atualizar o saldo.
                </DialogContentText>
                <Grid
                  container
                  spacing={2}
                  alignItems="center"
                  justify="space-between"
                >
                  <Grid item md={4}>
                    <DatePicker
                      name="date"
                      error={errors.date ? true : false}
                      helperText={errors.date}
                      setSelectedDate={handlerdDate}
                      date={props.search.date}
                      label="Data do Saldo"
                      disabled={true}
                      disableFuture={true}
                      disablePast={true}
                    />
                  </Grid>

                  <Grid item md={4}>
                    <BankAccountSelect
                      error={errors.bankAccount ? true : false}
                      helperText={errors.bankAccount}
                      handlerChangeSelect={handlerChange}
                    />
                  </Grid>
                  <Grid item md={4}>
                    <TextField
                      //className={classes.textfield}
                      type="text"
                      error={errors.balance ? true : false}
                      helperText={errors.balance}
                      //value={props.search.amount}
                      name="balance"
                      size="small"
                      label="Saldo Inicial:"
                      //value={column.format ? column.format(value) : value}
                      onChange={handlerTextChange}
                      variant="standard"
                      placeholder="Informe o Saldo Inicial"
                      inputProps={{
                        min: 0,
                        style: { textAlign: "end" },
                      }}
                      InputProps={{
                        inputComponent: NumberFormatCustom as any,
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
                    onSubmit(e);
                  }}
                  variant="contained"
                  color="primary"
                  classes={{ root: classes.button }}
                >
                  Salvar
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

export default BankStatementUpdatetModal;
