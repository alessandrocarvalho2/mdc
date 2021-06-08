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
import messageService from "../../../../../../core/services/message.service";
import React, { useEffect, useRef, useState } from "react";
import SnackBarTypeEnum from "../../../../../../core/enum/snackbar-type.enum";
import BankStatementService from "../../../../../../core/services/bank-statement.service";
import BankAccountSelect from "../../../../../../shared/components/ecash-bank-account-select/bank-account.component";
import EcashSnackbar from "../../../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import Spinner from "../../../../../../shared/components/ecash-spinner/ecash-spinner.component";
import bankStatementImportModelStyle from "./bank-statement-import-modal.style";

// eslint-disable-next-line @typescript-eslint/no-unused-vars
interface File {
  id: string;
  file: {
    data: any;
  };
}

interface BankStatementImportActionInterface {
  setOpen: any;
  setSearch: any;
  search: any;
  open: any;
}

const BankStatementImportModal = (
  props: BankStatementImportActionInterface
) => {
  const { setSearch, open, setOpen } = props;
  const [loading, setLoading] = useState(false);

  const classes = bankStatementImportModelStyle();
  const [isSelected, setIsSelected] = useState(false);
  const [selectedFile, setSelectedFile] = useState<any>();

  const bankStatementService = new BankStatementService();

  const onClose = () => {
    setOpen(false);
    setSearch({});
    setSelectedFile({ name: "" });
    setIsSelected(false);
  };

  const handlerChangeSelect = (name: any, value: any) => {
    props.setSearch({
      ...props.search,
      [name]: value && value.id ? value.id : "",
    });
  };

  const inputFile = useRef<HTMLInputElement>(null);

  const handleClick = () => {
    setSelectedFile({ name: "" });
    if (inputFile && inputFile.current) {
      inputFile.current.click();
    }
  };

  const handleFile = (e: React.FormEvent<HTMLInputElement>) => {
    e.preventDefault();
    const target = e.target as HTMLInputElement;
    const newFile = target.files;

    if (newFile) {
      setSelectedFile(newFile[0]);
      setIsSelected(true);
    }
  };

  const onSubmit = (e: any) => {
    e.preventDefault();

    if (validate()) {
      setLoading(true);

      const reader = new FormData();
      reader.append("file", selectedFile);
      bankStatementService
        .addFile(props.search.bankAccount, reader)
        .then((resp: any) => {
          snackBarOpen(
            messageService.success.MSG001.replace("{0}", "Extrato Bancário"),
            SnackBarTypeEnum.SUCCESS
          );
          setLoading(false);
          onClose();
        })
        .catch((errors) => {
          // react on errors.
          console.error(errors);
          setLoading(false);

          if (errors.response.data !== "") {
            snackBarOpen(
              errors.response.data.toString(),
              SnackBarTypeEnum.ERROR
            );
          } else if (errors.request) {
            snackBarOpen(
              messageService.error.MSG002.replace("{0}", "Extrato Bancário"),
              SnackBarTypeEnum.ERROR
            );
          } else {
            // Something happened in setting up the request and triggered an Error
            console.log("Error", errors.message);
            snackBarOpen(
              messageService.error.MSG002.replace("{0}", "Extrato Bancário"),
              SnackBarTypeEnum.ERROR
            );
          }
        });
    }
  };

  const initialFValues = {
    inputUpload: "",
    bankAccount: "",
  };

  const [errors, setErrors] = useState(initialFValues);

  const validate = () => {
    let temp = { ...errors };
    let isValid = true;
    if (selectedFile?.name === undefined || selectedFile?.name === "") {
      temp.inputUpload = "Informe um arquivo.";
      isValid = false;
    }
    if (
      props.search?.bankAccount === "" ||
      props.search?.bankAccount === undefined ||
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

    if (isSelected) {
      temp.inputUpload = "";
      setErrors({
        ...temp,
      });
    }
  };

  const [snackOpen, setSnackOpen] = useState(false);
  const [message, setMessage] = useState("");
  const [typeSnack, setTypeSnack] = useState(SnackBarTypeEnum.INFO);

  const snackBarOpen = (message: string, type: SnackBarTypeEnum) => {
    setSnackOpen(true);
    setMessage(message);
    setTypeSnack(type);
  };

  useEffect(() => {
    clearErrors();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.search?.bankAccount, selectedFile, isSelected]);

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
                Enviar Extrato Bancário
              </DialogTitle>
              <DialogContent>
                <DialogContentText classes={{ root: classes.text }}>
                  Importe o Extrato de Conta Bancária
                </DialogContentText>
                <Grid container spacing={4}>
                  <Grid item md={12}>
                    <BankAccountSelect
                      fullWidth
                      error={errors.bankAccount ? true : false}
                      helperText={errors.bankAccount}
                      handlerChangeSelect={handlerChangeSelect}
                    />
                  </Grid>
                  <Grid item md={6}>
                    <TextField
                      error={errors.inputUpload ? true : false}
                      name="inputUpload"
                      //label="Nome do Arquivo"
                      placeholder="Nome do Arquivo"
                      margin="normal"
                      value={selectedFile?.name}
                      helperText={errors.inputUpload}
                      InputProps={{
                        readOnly: true,
                      }}
                    />
                  </Grid>

                  <Grid item md={6}>
                    <input
                      className={classes.inputUpload}
                      type="file"
                      ref={inputFile}
                      onChange={(e) => handleFile(e)}
                    />
                    <Button
                      className={classes.buttonUpload}
                      variant="contained"
                      color="primary"
                      onClick={() => handleClick()}
                    >
                      Selecione o Arquivo
                    </Button>
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

export default BankStatementImportModal;
