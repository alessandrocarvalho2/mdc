import React, { useEffect, useRef, useState } from "react";
import { Grid, Icon, Button, Tooltip } from "@material-ui/core";
import BankTransactionHeaderStyles from "./bank-transaction-table-header.style";
import BankAccountSelect from "../../../../../shared/components/ecash-bank-account-select/bank-account.component";
import DatePicker from "../../../../../shared/components/ecash-date-picker/date-picker.component";
import moment from "moment";
import CategorySelect from "../../../../../shared/components/ecash-category-select/category.component";
import OperationSelect from "../../../../../shared/components/ecash-operation-select/operation.component";
import InOutSelect from "../../../../../shared/components/ecash-in-out-select/in-out.component";
import ApprovedSelect from "../../../../../shared/components/ecash-approved-select/approved.component";
import VisibleSelect from "../../../../../shared/components/ecash-visible-select/visible.component";
import IncludeSelect from "../../../../../shared/components/ecash-include-select/include.component";
import CashFlowService from "../../../../../core/services/cash-flow.service";
import EcashSnackbar from "../../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import SnackBarTypeEnum from "../../../../../core/enum/snackbar-type.enum";
import messageService from "../../../../../core/services/message.service";
import CategoryModel from "../../../../../core/models/category.model";
import OperationModel from "../../../../../core/models/operation.model";

const initialValueOperation: OperationModel = {
  id: undefined,
  code: "",
  description: "Todos",
};

const initialValueCategory: CategoryModel = {
  id: undefined,
  description: "Todos",
  bankAccountId: undefined,
  inOut: undefined,
};

const initialValueInOut = {
  id: undefined,
  description: "Todos",
};

const initialValueApproved = {
  id: undefined,
  description: "Todos",
};

const initialValueVisible = {
  id: true,
  description: "Visivel",
};

const initialValueInclude = {
  id: undefined,
  description: "Todos",
};


export default function BankTransactionTableHeader(props: any) {
  const classes = BankTransactionHeaderStyles();
  const cashFlowService = new CashFlowService();

  const handlerdDate = (dateStatement: Date | null) => {
    props.setSearch({
      ...props.search,
      date: dateStatement,
    });
  };

  const handlerChangeSelect = (name: any, value: any) => {
    props.setSearch({
      ...props.search,
      [name]:
        value !== undefined && value !== null && value !== "" ? value : "",
    });
  };

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
      props.search?.bankAccount.id === 0 ||
      props.search?.bankAccount.id === "" ||
      props.search?.bankAccount.id === undefined ||
      props.search?.bankAccount.id === null
    ) {
      temp.bankAccount = "Selecione uma Conta Corrente.";
      isValid = false;
    } else {
      temp.bankAccount = "";
      isValid = true;
    }

    setErrors({
      ...temp,
    });

    return isValid;
  };

  const clearErrors = () => {
    let temp = { ...errors };
    if (props.search?.bankAccount?.id > 0) {
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

  const onSubmit = () => {
    if (validate()) {
      props.onSubmit();
    }
  };

  const [searchAdvanced, setSearchAdvanced] = useState(false);

  const onViewSearchAdvanced = () => {
    setSearchAdvanced(!searchAdvanced);
  };

  const onClickTemplate = () => {
    cashFlowService
      .getTemplate()
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
              "Modelo"
            ),
            SnackBarTypeEnum.ERROR
          );
        } else {
          // Something happened in setting up the request and triggered an Error
          console.log("Error", errors.message);
          snackBarOpen(
            messageService.error.MSG009.replace("{0}", "Arquivo").replace(
              "{1}",
              "Modelo"
            ),
            SnackBarTypeEnum.ERROR
          );
        }
      })
      .finally(() => {});
  };

  const inputFile = useRef<HTMLInputElement>(null);
  const onClickImport = () => {
    console.log(inputFile.current);
    if (inputFile && inputFile.current) {
      inputFile.current.click();
    }
  };

  const handleFile = (e: any) => {
    e.preventDefault();
    const target = e.target as HTMLInputElement;
    const newFile = target.files;

    if (newFile) {
      const reader = new FormData();
      reader.append("file", newFile[0]);
      cashFlowService
        .upload(reader)
        .then((resp: any) => {
          snackBarOpen(
            messageService.success.MSG002.replace(
              "{0}",
              "Movimentação Bancária"
            ),
            SnackBarTypeEnum.SUCCESS
          );
          //inputFile = null;
        })
        .catch((errors) => {
          // react on errors.
          console.error(errors);

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
        })
        .finally(() => {
          e = null;
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



  const onClearFilters = () => {
    props.setSearch({
      ...props.search,
      category: initialValueCategory,
      operation: initialValueOperation,
      inOut: initialValueInOut,
      approved: initialValueApproved,
      visible: initialValueVisible,
      include: initialValueInclude,
    });
  };

  useEffect(() => {
    clearErrors();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.search?.bankAccount, props.search?.date]);

  return (
    <>
      <Grid container spacing={2} alignItems="center" justify="space-between">
        <Grid item md={12}>
          <span className={classes.title}>Movimentação de Caixa</span>
        </Grid>
      </Grid>
      <Grid
        container
        spacing={3}
        alignItems="center"
        justify="space-between"
        className={classes.filters}
      >
        <Grid container item md={3}>
          <BankAccountSelect
            error={errors.bankAccount ? true : false}
            helperText={errors.bankAccount}
            handlerChangeSelect={handlerChangeSelect}
          />
        </Grid>
        <Grid item md={3}>
          <DatePicker
            name="date"
            error={errors.date ? true : false}
            helperText={errors.date}
            setSelectedDate={handlerdDate}
            date={props.search.date}
            label="Data da Movimentação"
          />
        </Grid>
        <Grid
          item
          md={6}
          container
          spacing={2}
          alignItems="center"
          justify="space-between"
          className={classes.filters}
        >
          <Grid container item md={6}>
            <Button
              disableElevation
              variant="contained"
              color="primary"
              classes={{ label: classes.buttonText }}
              onClick={() => onSubmit()}
            >
              <Icon className={classes.icon}>search</Icon>
              Pesquisar
            </Button>
            <Tooltip title="Pesquisa Avançada" aria-label="publish">
              <Button
                disableElevation
                variant="contained"
                color="primary"
                classes={{ label: classes.buttonText }}
                onClick={() => onViewSearchAdvanced()}
              >
                <Icon className={classes.icon}>
                  {searchAdvanced ? "expand_less" : "expand_more"}
                </Icon>
              </Button>
            </Tooltip>
            <Grid container item md={1}>
              <Tooltip title="Limpar filtros" aria-label="publish">
                <Button
                  variant="contained"
                  color="default"
                  classes={{ label: classes.buttonText }}
                  onClick={() => onClearFilters()}
                >
                  <Icon className={classes.icon}>remove_circle_outline';</Icon>
                </Button>
              </Tooltip>
            </Grid>
          </Grid>
          <Grid container item md={3}>
            <Tooltip title="Exportar Template RCD" aria-label="publish">
              <Button
                disableElevation
                fullWidth
                variant="contained"
                color="secondary"
                classes={{ label: classes.buttonText }}
                onClick={() => onClickTemplate()}
              >
                <Icon className={classes.icon}>publish</Icon>
                Template
              </Button>
            </Tooltip>
          </Grid>
          <Grid container item md={3}>
            <input
              className={classes.inputUpload}
              type="file"
              ref={inputFile}
              onChange={(e) => handleFile(e)}
            />
            <Tooltip title="Importar Template RCD" aria-label="import">
              <Button
                disableElevation
                fullWidth
                variant="contained"
                color="secondary"
                classes={{ label: classes.buttonText }}
                onClick={() => onClickImport()}
              >
                <Icon className={classes.icon}>get_app</Icon>
                Importar
              </Button>
            </Tooltip>
          </Grid>
        </Grid>
      </Grid>
      <Grid
        container
        spacing={3}
        alignItems="center"
        justify="space-between"
        className={classes.filters}
      >
        <Grid
          container
          item
          md={2}
          className={searchAdvanced ? classes.displayShow : classes.displayHide}
        >
          <CategorySelect
            value={props.search.category}
            initialValue={initialValueCategory}
            error={false}
            helperText={""}
            handlerChangeSelect={handlerChangeSelect}
          />
        </Grid>
        <Grid
          container
          item
          md={2}
          className={searchAdvanced ? classes.displayShow : classes.displayHide}
        >
          <OperationSelect
            value={props.search.operation}
            initialValue={initialValueOperation}
            error={false}
            helperText={""}
            handlerChangeSelect={handlerChangeSelect}
          />
        </Grid>
        <Grid
          container
          item
          md={2}
          className={searchAdvanced ? classes.displayShow : classes.displayHide}
        >
          <InOutSelect
            value={props.search.inOut}
            initialValue={initialValueInOut}
            error={false}
            helperText={""}
            handlerChangeSelect={handlerChangeSelect}
          />
        </Grid>
        <Grid
          container
          item
          md={2}
          className={searchAdvanced ? classes.displayShow : classes.displayHide}
        >
          <ApprovedSelect
            value={props.search.approved}
            initialValue={initialValueApproved}
            error={false}
            helperText={""}
            handlerChangeSelect={handlerChangeSelect}
          />
        </Grid>
        <Grid
          container
          item
          md={2}
          className={searchAdvanced ? classes.displayShow : classes.displayHide}
        >
          <VisibleSelect
            value={props.search.visible}
            initialValue={initialValueVisible}
            error={false}
            helperText={""}
            handlerChangeSelect={handlerChangeSelect}
          />
        </Grid>
        <Grid
          container
          item
          md={2}
          className={searchAdvanced ? classes.displayShow : classes.displayHide}
        >
          <IncludeSelect
            value={props.search.include}
            initialValue={initialValueInclude}
            error={false}
            helperText={""}
            handlerChangeSelect={handlerChangeSelect}
          />
        </Grid>
      </Grid>

      <EcashSnackbar
        open={snackOpen}
        setOpen={setSnackOpen}
        type={typeSnack}
        message={message}
      />
    </>
  );
}
