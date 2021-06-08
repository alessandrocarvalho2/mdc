import React, { useEffect, useState } from "react";
import { Grid, Icon, Button } from "@material-ui/core";
import BankStatementHeaderStyles from "./bank-statement-table-header.style";
import BankAccountSelect from "../../../../../shared/components/ecash-bank-account-select/bank-account.component";
import DatePicker from "../../../../../shared/components/ecash-date-picker/date-picker.component";
import BankStatementImportModal from "./bank-statement-import-model/bank-statement-import-modal";
import moment from "moment";
import BankStatementRemovetModal from "./bank-statement-remove-model/bank-statement-remove-modal";
import ActionModal from "../../../../../shared/components/ecash-action-modal/action-modal.component";
import ModalTypeEnum from "../../../../../core/enum/modal-type.enum";
import messageService from "../../../../../core/services/message.service";
import BankStatementUpdatetModal from "./bank-statement-update-model/bank-statement-update-modal";
import AccountBalanceService from "../../../../../core/services/account-balance.service";
import EcashSnackbar from "../../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import SnackBarTypeEnum from "../../../../../core/enum/snackbar-type.enum";

export default function BankStatementHeaderHeader(props: any) {
  const classes = BankStatementHeaderStyles();
  const [bankStatementModalOpem, setBankStatementModalOpem] = useState(false);
  const accountBalanceService = new AccountBalanceService();
  const [
    bankStatementRemoveModalOpem,
    setBankStatementRemoveModalOpem,
  ] = useState(false);
  const [
    bankStatementUpdateModalOpen,
    setBankStatementUpdateModalOpen,
  ] = useState(false);
  const handleClickDialogOpen = (type: number) => {
    if (type === 0) setBankStatementModalOpem(true);
    else if (type === 1) setBankStatementRemoveModalOpem(true);
    else {
      if (isOpenCashFlow()) {
        setBankStatementUpdateModalOpen(true);
      } else {
        snackBarOpen(
          messageService.error.MSG006.replace("{0}", "Saldo Inicial"),
          SnackBarTypeEnum.ERROR
        );
      }
    }
  };

  const handlerdDate = (dateStatement: Date | null) => {
    props.setSearch({
      ...props.search,
      date: dateStatement,
    });
  };

  const handlerChangeSelect = (name: any, value: any) => {
    props.setSearch({
      ...props.search,
      [name]: value && value.id ? value.id : "",
    });
  };

  const [search, setSearch] = useState<any>({});
  const initialErros = {
    date: "",
    bankAccount: "",
  };

  const [errors, setErrors] = useState(initialErros);

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

  const onSubmit = () => {
    if (validate()) {
      props.onSubmit();
    }
  };

  const [openModal, setOpenModal] = useState(false);
  const [typeModal, setTypeModal] = useState<ModalTypeEnum>();
  const [titleModal, setTitleModal] = useState("");
  const [message, setMessage] = useState("");

  const handlerModal = () => {
    accountBalanceService
      .isAllowedToSave(new Date())
      .then((data: any) => {
        const isOpenCashFlow = data as Boolean;
        if (isOpenCashFlow) {
          setTypeModal(ModalTypeEnum.CONFIRM);
          setOpenModal(true);
          setMessage(messageService.info.MSG004);
          setTitleModal("Atenção");
        } else {
          snackBarOpen(
            messageService.info.MSG005,
            SnackBarTypeEnum.WARNING
          );
        }
      })
      .catch((errors) => {
        if (errors.response) {
          // console.log(error.response.data);
          snackBarOpen(errors.response.data, SnackBarTypeEnum.ERROR);
        } else if (errors.request) {
          snackBarOpen(
            messageService.error.MSG003.replace("{0}", "Saldo inicial"),
            SnackBarTypeEnum.ERROR
          );
        } else {
          // Something happened in setting up the request and triggered an Error
          console.log("Error", errors.message);
        }
      });
  };

  const handlerDeniedModal = () => {
    setOpenModal(false);
  };

  const handleClickConfirmModal = () => {
    setOpenModal(false);
    handleClickDialogOpen(2);
  };

  const isOpenCashFlow = (): Boolean => {
    let resp = true;

    accountBalanceService
      .isAllowedToSave(new Date())
      .then((data: any) => {
        resp = data;
      })
      .catch((errors) => {
        resp = false;
      });

    return resp;
  };

  const [snackOpen, setSnackOpen] = useState(false);
  const [typeSnack, setTypeSnack] = useState(SnackBarTypeEnum.INFO);

  const snackBarOpen = (message: string, type: SnackBarTypeEnum) => {
    setSnackOpen(true);
    setMessage(message);
    setTypeSnack(type);
  };

  return (
    <>
      <Grid container spacing={2} alignItems="center" justify="space-between">
        <Grid item md={6}>
          <span className={classes.title}>Extrato Bancário</span>
        </Grid>
        <Grid item md={2}>
          <Button
            disableElevation
            fullWidth
            variant="contained"
            color="secondary"
            classes={{ label: classes.buttonText }}
            onClick={() => handleClickDialogOpen(0)}
          >
            <Icon className={classes.icon}>get_app</Icon>
            Importar
          </Button>
        </Grid>
        <Grid item md={2}>
          <Button
            disableElevation
            fullWidth
            variant="contained"
            color="secondary"
            classes={{ label: classes.buttonText }}
            onClick={() => handlerModal()}
          >
            <Icon className={classes.icon}>attach_money</Icon>
            Atualizar
          </Button>
        </Grid>
        <Grid item md={2}>
          <Button
            disableElevation
            fullWidth
            variant="contained"
            color="secondary"
            classes={{ label: classes.buttonText }}
            onClick={() => handleClickDialogOpen(1)}
          >
            <Icon className={classes.icon}>delete</Icon>
            Excluir
          </Button>
        </Grid>
      </Grid>
      <Grid
        container
        spacing={3}
        alignItems="center"
        justify="space-between"
        className={classes.filters}
      >
        <Grid item md={5}>
          <BankAccountSelect
            error={errors.bankAccount ? true : false}
            helperText={errors.bankAccount}
            handlerChangeSelect={handlerChangeSelect}
          />
        </Grid>
        <Grid item md={5}>
          <DatePicker
            name="date"
            error={errors.date ? true : false}
            helperText={errors.date}
            setSelectedDate={handlerdDate}
            date={props.search.date}
            label="Data do Extrato"
          />
        </Grid>
        <Grid item md={2}>
          <Button
            disableElevation
            fullWidth
            variant="contained"
            color="primary"
            classes={{ label: classes.buttonText }}
            onClick={() => onSubmit()}
          >
            <Icon className={classes.icon}>search</Icon>
            Pesquisar
          </Button>
        </Grid>
      </Grid>
      <BankStatementImportModal
        open={bankStatementModalOpem}
        setOpen={setBankStatementModalOpem}
        search={search}
        setSearch={setSearch}
      />
      <BankStatementRemovetModal
        open={bankStatementRemoveModalOpem}
        setOpen={setBankStatementRemoveModalOpem}
        search={search}
        setSearch={setSearch}
      />
      <BankStatementUpdatetModal
        open={bankStatementUpdateModalOpen}
        setOpen={setBankStatementUpdateModalOpen}
        search={search}
        setSearch={setSearch}
      />
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
    </>
  );
}
