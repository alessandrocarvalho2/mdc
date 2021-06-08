import {
  Grid,
  Button,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  IconButton,
  Dialog,
} from "@material-ui/core";
import { Close } from "@material-ui/icons";
import moment from "moment";
import React, { useEffect,useState } from "react";
import SnackBarTypeEnum from "../../../../../../core/enum/snackbar-type.enum";
import BankStatementService from "../../../../../../core/services/bank-statement.service";
import messageService from "../../../../../../core/services/message.service";
import BankAccountSelect from "../../../../../../shared/components/ecash-bank-account-select/bank-account.component";
import DatePicker from "../../../../../../shared/components/ecash-date-picker/date-picker.component";
import EcashSnackbar from "../../../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import Spinner from "../../../../../../shared/components/ecash-spinner/ecash-spinner.component";
import bankStatementImportModelStyle from "./bank-statement-remove-modal.style";

interface BankStatementRemoveActionInterface {
  setOpen: any;
  setSearch: any;
  search: any;
  open: any;
}

const BankStatementRemovetModal = (
  props: BankStatementRemoveActionInterface
) => {
  const { setSearch, open, setOpen } = props;

  const classes = bankStatementImportModelStyle();
  const [loading, setLoading] = useState(false);

  const bankStatementService = new BankStatementService();

  const onClose = () => {
    setOpen(false);
    setSearch({});
    props.setSearch({});
  };

  const handlerChange = (name: any, value: any) => {
    props.setSearch({
      ...props.search,
      [name]: value && value.id ? value.id : "",
    });
  };

  const handlerdDate = (dateStatement: Date | null) => {
    props.setSearch({
      ...props.search,
      date: dateStatement,
    });
  };

  const onSubmit = () => {
    if (validate()) {
      setLoading(true);

      bankStatementService
        .deleteByBankAndDate(props.search.bankAccount, props.search.date)
        .then((resp: any) => {
          setLoading(false);
          snackBarOpen(
            messageService.success.MSG004.replace("{0}", "Extrato Bancário"),
            SnackBarTypeEnum.SUCCESS
          );
          setOpen(false);
        })
        .catch((errors) => {
          console.error(errors);
          setLoading(false);
          snackBarOpen(
            messageService.error.MSG005.replace("{0}", "Extrato Bancário"),
            SnackBarTypeEnum.ERROR
          );
        });
    }
  };

  const initialFValues = {
    date: "",
    bankAccount: "",
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
  };

  useEffect(() => {
    clearErrors();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.search?.bankAccount, props.search?.date]);

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
                Excluir Extrato Bancário
              </DialogTitle>
              <DialogContent>
                <DialogContentText classes={{ root: classes.text }}>
                  {" "}
                  Informe a Conta Corrente e Data que foi importado o Extrato
                </DialogContentText>
                <Grid
                  container
                  spacing={2}
                  alignItems="center"
                  justify="space-between"
                >
                  <Grid item md={6}>
                    <BankAccountSelect
                      error={errors.bankAccount ? true : false}
                      helperText={errors.bankAccount}
                      handlerChangeSelect={handlerChange}
                    />
                  </Grid>
                  <Grid item md={6}>
                    <DatePicker
                      name="date"
                      error={errors.date ? true : false}
                      helperText={errors.date}
                      setSelectedDate={handlerdDate}
                      date={props.search.date}
                      label="Data do Extrato"
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
                  onClick={() => {
                    onSubmit();
                    //toggleForm();
                  }}
                  //onClick={() => props.handleClickSelect(props.choices[1].name)}
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

export default BankStatementRemovetModal;
