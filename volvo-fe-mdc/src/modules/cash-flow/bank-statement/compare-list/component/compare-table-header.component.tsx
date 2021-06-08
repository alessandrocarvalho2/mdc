import React, { useEffect, useState } from "react";
import {
  Grid,
  Icon,
  Button,
  IconButton,
  TextField,
  Tooltip,
} from "@material-ui/core";
import compareHeaderStyles from "./compare-table-header.style";
import BankAccountSelect from "../../../../../shared/components/ecash-bank-account-select/bank-account.component";
import DatePicker from "../../../../../shared/components/ecash-date-picker/date-picker.component";
import moment from "moment";
import ActionModal from "../../../../../shared/components/ecash-action-modal/action-modal.component";
import ModalTypeEnum from "../../../../../core/enum/modal-type.enum";
import messageService from "../../../../../core/services/message.service";
import NumberFormatCustom from "../../../../../shared/components/ecash-number-format/number-format-custom.component";
import BankTransactionAddModel from "./bank-transaction-add-modal/bank-transaction-add-modal.component";
import validation from "../../../../../core/utils/validation.util";

const ComparaTableHeader = (props: any) => {
  const classes = compareHeaderStyles();
  const [typeUndo, setTypeUndo] = useState(0);

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
    const formatOk = validation.isDate(
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

  const calculate = (): number => {
    const balance =
      parseFloat(props.fields?.statementAmount ?? 0) -
      parseFloat(props.fields?.transactionAmount ?? 0);

    setAddTransaction({
      ...addTransaction,
      date: props.search.date,
      bankAccount: props.search.bankAccount,
      balance: balance,
      description: props.fields?.description,
      inOut: balance > 0 ? "in" : balance < 0 ? "out" : "",
    });

    return balance;
  };

  const onSubmit = () => {
    clearFields();
    if (validate()) {
      props.onSubmit();
    }
  };

  const undo = () => {
    if (validate()) {
      props.undo();
    }
  };

  const undoAll = () => {
    if (validate()) {
      props.undoAll();
    }
  };

  const [openModal, setOpenModal] = useState(false);
  const [typeModal, setTypeModal] = useState<ModalTypeEnum>();
  const [titleModal, setTitleModal] = useState("");
  const [message, setMessage] = useState("");

  const initialFields = {
    checksStatement: "",
    checksTransaction: "",
  };

  const [fields, setFields] = useState(initialFields);

  const initialaddTransaction = {
    date: "",
    bankAccount: "",
    description: "",
    inOut: "",
    operation: undefined,
    category: undefined,
    balance: 0,
  };

  const [addTransaction, setAddTransaction] = useState(initialaddTransaction);
  const [addTransactionOpen, setAddTransactionOpen] = useState(false);

  const clearFields = () => {
    setFields(initialFields);
  };

  const onAddTransaction = () => {
    if (validate()) {
      if (calculate() !== 0) {
        setAddTransaction({
          ...addTransaction,
          description: addTransaction.description,
          operation: undefined,
          category: undefined,
        });
        setAddTransactionOpen(true);
      } else {
        props.onSaveConciliation();
      }
    }
  };

  const [searchAdvanced, setSearchAdvanced] = useState(false);

  const onViewSearchAdvanced = () => {
    setSearchAdvanced(!searchAdvanced);
  };

  const handlerChange = (event: any) => {
    const name = event.target.name;
    const value = event.target.value;
    setFields({
      ...fields,
      [name]: value,
    });
  };

  const [errorCheckStatement, setErrorsCheckStatement] = useState("");
  const [errorCheckCash, setErrorsCheckCash] = useState("");

  const getIndexCheckedStatement = () => {
    if (validation.isOnlyNumberHyphensSemicolon(fields.checksStatement)) {
      let checks = getIndex(fields.checksStatement);

      const data = props.dataStatement.map((item: any, index: number) => {
        if (checks.filter((x: any) => x === index + 1) > 0) {
          const updatedItem = {
            ...item,
            checkedStatement: true,
          };

          return updatedItem;
        }

        return item;
      });
      props.setDataStatement(data);
      setErrorsCheckStatement("");
    } else setErrorsCheckStatement('Informa somente Numéros, " - ", " : "');
  };

  const getIndexCheckedCash = () => {
    if (validation.isOnlyNumberHyphensSemicolon(fields.checksTransaction)) {
      let checks = getIndex(fields.checksTransaction);

      const data = props.dataCash.map((item: any, index: number) => {
        if (checks.filter((x: any) => x === index + 1) > 0) {
          const updatedItem = {
            ...item,
            checkedCash: true,
          };

          return updatedItem;
        }

        return item;
      });
      props.setDataCash(data);
      setErrorsCheckCash("");
    } else setErrorsCheckCash('Informa somente Numéros, " - ", " : "');
  };

  const getIndex = (text: string): any => {
    const fragmentText = text.split("");

    let indexsChecked: any = [];
    let lastCaracter: any;
    let numCaracter: any;
    let count = 0;
    for (let index = 0; index < fragmentText.length; index++) {
      const element = fragmentText[index];
      count++;
      if (validation.isNumber(element)) {
        numCaracter = !numCaracter
          ? element.toString()
          : numCaracter.toString() + element.toString();

        if (count === fragmentText.length) {
          if (lastCaracter === ";") {
            indexsChecked.push(parseInt(numCaracter));
            numCaracter = undefined;
            lastCaracter = undefined;
          } else if (lastCaracter === "-") {
            if (validation.isNumber(numCaracter)) {
              indexsChecked = addRange(
                indexsChecked,
                parseInt(indexsChecked[indexsChecked.length - 1]),
                parseInt(numCaracter)
              );
            }
          } else {
            indexsChecked.push(parseInt(numCaracter));
          }
        }
      } else {
        if (!lastCaracter) {
          lastCaracter = element;
          indexsChecked.push(parseInt(numCaracter));
          numCaracter = undefined;
        } else {
          lastCaracter = lastCaracter ?? element;
          if (lastCaracter === ";") {
            indexsChecked.push(parseInt(numCaracter));
            numCaracter = undefined;
            lastCaracter = undefined;
          } else if (lastCaracter === "-") {
            if (validation.isNumber(numCaracter)) {
              indexsChecked = addRange(
                indexsChecked,
                parseInt(indexsChecked[indexsChecked.length - 1]),
                parseInt(numCaracter)
              );
              numCaracter = undefined;
              lastCaracter = undefined;
            }
          }
          lastCaracter = element;
        }
      }
    }
    indexsChecked = indexsChecked.filter((x: any) => validation.isNumber(x));
    return indexsChecked;
  };

  const addRange = (
    indexsChecked: any = [],
    startNumber: any,
    lastNumber: any
  ) => {
    if (startNumber > lastNumber) {
      const tempStartNumber = startNumber;
      const tempLastNumber = lastNumber;
      startNumber = tempLastNumber;
      lastNumber = tempStartNumber;
    }
    indexsChecked.pop();
    for (let index = startNumber; index <= lastNumber; index++) {
      indexsChecked.push(index);
    }

    return indexsChecked;
  };

  const handlerModal = (type: number) => {
    if (validate()) {
      setTypeUndo(type);
      setTypeModal(ModalTypeEnum.CONFIRM);
      setOpenModal(true);
      if (type === 1) setMessage(messageService.info.MSG007);
      else setMessage(messageService.info.MSG008);
      setTitleModal("Atenção");
    }
  };

  const handleClickConfirmModal = () => {
    setOpenModal(false);
    if (typeUndo === 1) undo();
    else undoAll();
  };

  const handlerDeniedModal = () => {
    setOpenModal(false);
  };

  useEffect(() => {
    calculate();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.fields?.statementAmount]);

  useEffect(() => {
    calculate();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.fields?.statementAmount, props.fields?.transactionAmount]);

  useEffect(() => {
    clearErrors();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [props.search?.bankAccount, props.search?.date]);

  return (
    <>
      <Grid container spacing={2} alignItems="center" justify="space-between">
        <Grid item md={6}>
          <span className={classes.title}>Conciliação</span>
        </Grid>
        <Grid item md={6}></Grid>
      </Grid>
      <Grid
        item
        container
        alignItems="center"
        justify="space-between"
        spacing={2}
        className={classes.filters}
      >
        <Grid
          container
          alignItems="center"
          justify="space-between"
          item
          md={6}
          spacing={2}
        >
          <Grid item md={5}>
            <BankAccountSelect
              error={errors.bankAccount ? true : false}
              helperText={errors.bankAccount}
              handlerChangeSelect={handlerChangeSelect}
            />
          </Grid>
          <Grid item md={4}>
            <DatePicker
              name="date"
              error={errors.date ? true : false}
              helperText={errors.date}
              setSelectedDate={handlerdDate}
              date={props.search.date}
              label="Data:"
            />
          </Grid>
          <Grid item md={3}>
            <Grid
              container
              alignItems="center"
              justify="space-between"
              item
              spacing={0}
            >
              <Grid item container md={6}>
                <Button
                  fullWidth
                  disableElevation
                  variant="contained"
                  color="primary"
                  classes={{ label: classes.buttonText }}
                  onClick={() => onSubmit()}
                >
                  <Icon>search</Icon>
                </Button>
              </Grid>
              <Grid item container md={6}>
                <Button
                  fullWidth
                  disableElevation
                  variant="contained"
                  color="primary"
                  classes={{ label: classes.buttonText }}
                  onClick={() => onViewSearchAdvanced()}
                >
                  <Icon>{searchAdvanced ? "expand_less" : "expand_more"}</Icon>
                </Button>
              </Grid>
            </Grid>
          </Grid>
        </Grid>
        <Grid
          item
          container
          alignItems="center"
          justify="space-between"
          md={6}
          spacing={0}
        >
          <Grid
            item
            container
            spacing={0}
            alignItems="center"
            justify="space-between"
          >
            <Grid item md={1}>
              <Tooltip title="Desfazer ultima conciliação" aria-label="add">
                <div>
                  <IconButton
                    onClick={() => handlerModal(1)}
                    aria-label="delete"
                  >
                    <Icon>reply</Icon>
                  </IconButton>
                </div>
              </Tooltip>
            </Grid>
            <Grid item md={1}>
              <Tooltip title="Desfazer todas conciliações" aria-label="add">
                <div>
                  <IconButton
                    onClick={() => handlerModal(2)}
                    aria-label="delete"
                  >
                    <Icon>reply_all</Icon>
                  </IconButton>
                </div>
              </Tooltip>
            </Grid>
            <Grid item container spacing={2} md={9}>
              <Grid
                item
                container
                spacing={0}
                alignItems="center"
                justify="space-between"
              >
                <Grid item md={3}>
                  <TextField
                    type="text"
                    name="statement"
                    size="small"
                    label="Extrato:"
                    value={props.fields.statementAmount}
                    variant="standard"
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
                <Grid item md={1} container justify="center">
                  <Icon>remove</Icon>
                </Grid>
                <Grid item md={3}>
                  <TextField
                    type="text"
                    name="transaction"
                    size="small"
                    label="Caixa:"
                    value={props.fields.transactionAmount}
                    variant="standard"
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
                <Grid item md={1} container justify="center">
                  <Icon>drag_handle</Icon>
                </Grid>
                <Grid item md={4}>
                  <TextField
                    type="text"
                    name="balance"
                    size="small"
                    label="Ajuste de Caixa:"
                    value={props.fields.balanceAmount}
                    variant="standard"
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
            </Grid>
            <Grid item container md={1}>
              <Tooltip title="Conciliar" aria-label="add">
                <Button
                  fullWidth
                  disableElevation
                  variant="contained"
                  color="primary"
                  classes={{ label: classes.buttonText }}
                  onClick={() => onAddTransaction()}
                >
                  <Icon>autorenew</Icon>
                </Button>
              </Tooltip>
            </Grid>
          </Grid>
        </Grid>
      </Grid>
      <Grid
        item
        container
        spacing={2}
        alignItems="center"
        justify="space-between"
        className={searchAdvanced ? classes.displayShow : classes.displayHide}
      >
        <Grid
          item
          container
          spacing={2}
          md={6}
          alignItems="center"
          justify="space-between"
          className={searchAdvanced ? classes.displayShow : classes.displayHide}
        >
          <Grid item md={9}>
            <TextField
              type="text"
              value={fields.checksStatement}
              name="checksStatement"
              error={errorCheckStatement ? true : false}
              helperText={errorCheckStatement}
              size="small"
              label="Marcar Itens Extrato Bancário:"
              onChange={(e) => handlerChange(e)}
              variant="standard"
            />
          </Grid>
          <Grid
            item
            md={3}
            className={
              searchAdvanced ? classes.displayShow : classes.displayHide
            }
          >
            <Button
              fullWidth
              disableElevation
              variant="contained"
              color="primary"
              classes={{ label: classes.buttonText }}
              onClick={() => getIndexCheckedStatement()}
            >
              <Icon className={classes.icon}>check</Icon>
              Marcar
            </Button>
          </Grid>
        </Grid>
        <Grid
          item
          container
          spacing={2}
          md={6}
          alignItems="center"
          justify="space-between"
          className={searchAdvanced ? classes.displayShow : classes.displayHide}
        >
          <Grid item md={9}>
            <TextField
              type="text"
              value={fields.checksTransaction}
              name="checksTransaction"
              error={errorCheckCash ? true : false}
              helperText={errorCheckCash}
              size="small"
              label="Marcar Itens Extrato Cash:"
              onChange={(e) => handlerChange(e)}
              variant="standard"
            />
          </Grid>
          <Grid
            item
            md={3}
            className={
              searchAdvanced ? classes.displayShow : classes.displayHide
            }
          >
            <Button
              fullWidth
              disableElevation
              variant="contained"
              color="primary"
              classes={{ label: classes.buttonText }}
              onClick={() => getIndexCheckedCash()}
            >
              <Icon className={classes.icon}>check</Icon>
              Marcar
            </Button>
          </Grid>
        </Grid>
      </Grid>
      <BankTransactionAddModel
        open={addTransactionOpen}
        setOpen={setAddTransactionOpen}
        onSubmit={props.onSubmit}
        setAddTransaction={setAddTransaction}
        addTransaction={addTransaction}
        dataStatement={props.dataStatement}
        dataCash={props.dataCash}
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

export default ComparaTableHeader;
