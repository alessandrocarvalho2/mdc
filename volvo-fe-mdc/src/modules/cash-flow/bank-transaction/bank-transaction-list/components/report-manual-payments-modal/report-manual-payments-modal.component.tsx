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
import React, { useEffect, useState } from "react";
import ModalTypeEnum from "../../../../../../core/enum/modal-type.enum";
import SnackBarTypeEnum from "../../../../../../core/enum/snackbar-type.enum";
import CashFlowDetailedModel from "../../../../../../core/models/cash-flow-detailed.model";
import Columns from "../../../../../../core/models/columns-table.model";
import messageService from "../../../../../../core/services/message.service";
import ActionModal from "../../../../../../shared/components/ecash-action-modal/action-modal.component";
import NumberFormatCustomWithOutNegative from "../../../../../../shared/components/ecash-number-format/number-format-custom-without-negative.component";
import EcashSnackbar from "../../../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import EcashTableBasicList from "../../../../../../shared/components/ecash-table-basic/ecash-table-basic.component";
import reportManualPaymentsModalStyle from "./report-manual-payments-modal.style";

interface ReportManualPaymentsModalInterface {
  open: any;
  setOpen: any;
  dataCashFlow: any;
  setDataCashFlow: any;
  data: any;
  setData: any;
}

let columns: Columns[] = [
  {
    id: "checked",
    label: "checked",
    align: "center",
    type: "checkbox",
    witnCheckedAll: true,
  },
  {
    id: "documentName",
    label: "Nº Documento ",
    align: "left",
    type: "string",
  },
  {
    id: "amount",
    label: "Valor ",
    align: "right",
    type: "string",
    format: (value: any) =>
      value.toLocaleString("pt-BR", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }),
  },
  {
    id: "detailedDescription",
    label: "Observações",
    align: "left",
    type: "string",
  },
  {
    id: "index",
    label: "Ações",
    align: "center",
    type: "button",
    children: [
      {
        id: "isDetailedTransaction",
        label: "Editar",
        align: "center",
        type: "button",
        icon: "edit",
      },
    ],
  },
];

const initialFValues = {
  id: "",
  cashFlowId: "",
  documentName: "",
  amount: "",
  detailedDescription: "",
};

const ReportManualPaymentsModal = (
  props: ReportManualPaymentsModalInterface
) => {
  const { open, setOpen, dataCashFlow, setDataCashFlow, data, setData } = props;

  const classes = reportManualPaymentsModalStyle();

  const [cashFlowDetailed, setCashFlowDetailed] =
    useState<CashFlowDetailedModel>({
      id: 0,
      cashFlowId: 0,
      documentName: "",
      amount: 0,
      detailedDescription: "",
    });
  const [dataCashFlowDetailed, setDataCashFlowDetailed] = useState<
    CashFlowDetailedModel[]
  >([]);

  const [dataCashFlowDetailedUpdate, setDataCashFlowDetailedUpdate] = useState<
    CashFlowDetailedModel[]
  >([]);

  const [selected, setSelected] = React.useState<any[]>([]);
  const [amount, setAmount] = useState(0);
  const onClose = () => {
    setOpen(false);
    setCashFlowDetailed({
      id: 0,
      cashFlowId: 0,
      documentName: "",
      amount: 0,
      detailedDescription: "",
    });
  };

  const handlerTextChange = (event: any) => {
    const name = event.target.name;
    const value = event.target.value;

    setCashFlowDetailed({
      ...cashFlowDetailed,
      [name]: name === "amount" ? parseFloat(value) : value,
    });
  };

  const [errors, setErrors] = useState(initialFValues);

  const validate = () => {
    let temp = { ...errors };
    let isValid = true;

    if (
      cashFlowDetailed?.documentName === "" ||
      cashFlowDetailed?.documentName === undefined ||
      cashFlowDetailed?.documentName === null
    ) {
      temp.documentName = "Informe o nº do documento.";
      isValid = false;
    }

    if (
      cashFlowDetailed?.amount === undefined ||
      cashFlowDetailed?.amount === null ||
      cashFlowDetailed?.amount === 0 ||
      isNaN(cashFlowDetailed?.amount) 
    ) {
      temp.amount = "Informe o valor.";
      isValid = false;
    }

    setErrors({
      ...temp,
    });

    return isValid;
  };

  const clearErrors = () => {
    let temp = { ...errors };

    if (
      cashFlowDetailed?.amount !== undefined &&
      cashFlowDetailed?.amount !== null &&
      cashFlowDetailed?.amount > 0
    ) {
      temp.amount = "";
      setErrors({
        ...temp,
      });
    }

    if (
      cashFlowDetailed?.documentName !== "" &&
      cashFlowDetailed?.documentName !== undefined &&
      cashFlowDetailed?.documentName !== null
    ) {
      temp.documentName = "";
      setErrors({
        ...temp,
      });
    }

    if (
      cashFlowDetailed?.detailedDescription !== "" &&
      cashFlowDetailed?.detailedDescription !== undefined &&
      cashFlowDetailed?.detailedDescription !== null
    ) {
      temp.detailedDescription = "";
      setErrors({
        ...temp,
      });
    }
  };

  useEffect(() => {
    clearErrors();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [
    cashFlowDetailed?.amount,
    cashFlowDetailed?.documentName,
    cashFlowDetailed?.detailedDescription,
  ]);

  const [snackOpen, setSnackOpen] = useState(false);
  const [message, setMessage] = useState("");
  const [typeSnack, setTypeSnack] = useState(SnackBarTypeEnum.INFO);

  const snackBarOpen = (message: string, type: SnackBarTypeEnum) => {
    setSnackOpen(true);
    setMessage(message);
    setTypeSnack(type);
  };

  const onDelete = () => {
    const newData = dataCashFlowDetailed.map((item: any) => {
      if (selected.some((x: any) => x.documentName === item.documentName)) {
        const updatedItem = {
          ...item,
          delete: true,
        };

        return updatedItem;
      }
      return item;
    });

    setSelected([]);

    setDataCashFlowDetailedUpdate(newData);
  };

  const handleClick = (event: any) => {
    const anchor = ((event.target as HTMLDivElement).ownerDocument || document).querySelector(
      '#back-to-top-anchor',
    );

    if (anchor) {
      anchor.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
  };

  const onEdit = (row: any, event?: React.MouseEvent<HTMLDivElement>) => {
    row.isChanged = true;
    setCashFlowDetailed(row);
    handleClick(event);

  };

  const onAdd = (e: any) => {
    e.preventDefault();

    if (validate()) {
      if (cashFlowDetailed?.isChanged) {
        if (
          !dataCashFlowDetailed.some(
            (x) =>
              x.documentName?.trim() ===
                cashFlowDetailed?.documentName?.trim() && x.isChanged === false
          )
        ) {
          const data = dataCashFlowDetailed.map((item: any, index: number) => {
            if (item.documentName === cashFlowDetailed.documentName) {
              return cashFlowDetailed;
            }
            return item;
          });

          setCashFlowDetailed({
            id: 0,
            cashFlowId: 0,
            documentName: "",
            amount: 0,
            detailedDescription: "",
          });
          setDataCashFlowDetailedUpdate(data);
        } else {
          snackBarOpen(messageService.info.MSG009, SnackBarTypeEnum.WARNING);
        }
      } else {
        if (
          !dataCashFlowDetailed.some(
            (x) =>
              x.documentName?.trim() ===
                cashFlowDetailed?.documentName?.trim() && x.delete === false
          )
        ) {
          let item = cashFlowDetailed;
          item.delete = false;
          let items = dataCashFlowDetailed;
          items?.push(cashFlowDetailed);

          const data = items.map((item: any, index: number) => {
            item.index = index;
            return item;
          });

          setCashFlowDetailed({
            id: 0,
            cashFlowId: 0,
            documentName: "",
            amount: 0,
            detailedDescription: "",
          });
          setDataCashFlowDetailedUpdate(data);
        } else {
          snackBarOpen(messageService.info.MSG009, SnackBarTypeEnum.WARNING);
        }
      }
    }
  };

  const onFinish = (item?: any) => {
    const dateNew = data.map((item: any, index: number) => {
      if (item.index === dataCashFlow.index) {
        const updatedItem = {
          ...item,
          isChanged: true,
          cashFlowDetaileds: dataCashFlowDetailed,
        };
        setDataCashFlow(updatedItem);

        return updatedItem;
      }
      return item;
    });

    setData(dateNew);
    snackBarOpen(
      messageService.info.MSG011.replace("{0}", "Movimentação Bancária"),
      SnackBarTypeEnum.INFO
    );

    onClose();
  };

  useEffect(() => {
    const data = dataCashFlow?.cashFlowDetaileds
      ?.filter((x: any) => !x.delete)
      .map((item: any, index: number) => {
        item.index = index;
        item.delete = false;
        return item;
      });

    setDataCashFlowDetailed(data);
    setDataCashFlowDetailedUpdate(data);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [dataCashFlow?.cashFlowDetaileds]);

  useEffect(() => {
    setAmount(
      dataCashFlowDetailedUpdate
        ?.filter((x: any) => !x.delete)
        .reduce(
          (sum: number, current: any) => sum + (current ? current.amount : 0),
          0
        )
    );

    const data = dataCashFlowDetailedUpdate?.map((item: any, index: number) => {
      return item;
    });

    setDataCashFlowDetailed(data);

    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [dataCashFlowDetailedUpdate]);

  const [openModal, setOpenModal] = useState(false);
  const [typeModal, setTypeModal] = useState<ModalTypeEnum>();
  const [titleModal, setTitleModal] = useState("");

  const handlerModal = () => {
    setTypeModal(ModalTypeEnum.CONFIRM);
    setOpenModal(true);
    setMessage(messageService.info.MSG010);
    setTitleModal("Atenção");
  };

  const handlerDeniedModal = () => {
    setOpenModal(false);
  };

  const handleClickConfirmModal = () => {
    setOpenModal(false);
    onDelete();
  };
  
  return (
    <div  >
      <Dialog
        open={open}
        onClose={() => onClose()}
        fullWidth={true}
        maxWidth={"lg"}
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
         {"Manter lançamentos de pagamentos detalhados"}
        </DialogTitle>
        <DialogContent>
          <DialogContentText classes={{ root: classes.text }}>
            {" "}
          </DialogContentText>
          <div id="back-to-top-anchor"/>
          <Grid
            container
            item
            spacing={2}
            alignItems="center"
            justify="space-between"
          >
            <Grid container spacing={2} item md={12}>
           
              <Grid item md={6}>
           
                <TextField
                  type="text"
                  value={cashFlowDetailed?.documentName}
                  error={errors.documentName ? true : false}
                  helperText={errors.documentName}
                  name="documentName"
                  size="small"
                  label="Nº do documento:"
                  onChange={handlerTextChange}
                  variant="standard"
                  placeholder="Informe o Número do Documento."
                  inputProps={{
                    maxLength: "50",
                  }}
                />
              </Grid>
              <Grid item md={6}>
                <TextField
                  type="text"
                  value={cashFlowDetailed?.amount}
                  error={errors.amount ? true : false}
                  helperText={errors.amount}
                  name="amount"
                  size="small"
                  label="Valor:"
                  onChange={handlerTextChange}
                  variant="standard"
                  placeholder="Informe o Valor."
                  inputProps={{
                    min: 0,
                    //maxLength: "15",
                    style: { textAlign: "end" },
                  }}
                  InputProps={{
                    inputComponent: NumberFormatCustomWithOutNegative as any,
                  }}
                />
              </Grid>
            </Grid>
            <Grid container spacing={2} item md={12}>
              <Grid item md={10}>
                <TextField
                  type="text"
                  value={cashFlowDetailed?.detailedDescription}
                  error={errors.detailedDescription ? true : false}
                  helperText={errors.detailedDescription}
                  name="detailedDescription"
                  size="small"
                  label="Observações:"
                  onChange={handlerTextChange}
                  variant="standard"
                  placeholder="Informe a Observações."
                  inputProps={{
                    maxLength: "50",
                  }}
                />
              </Grid>
              <Grid item md={2}>
                <Button
                  fullWidth
                  onClick={(e) => {
                    onAdd(e);
                  }}
                  variant="contained"
                  color="primary"
                  classes={{ root: classes.button }}
                >
                  Incluir
                </Button>
              </Grid>
            </Grid>
            <Grid item md={12}>
              <EcashTableBasicList
                totalAmount={amount}
                data={dataCashFlowDetailed?.filter((x: any) => !x.delete)}
                selected={selected}
                setSelected={setSelected}
                columns={columns}
                actionButtonAfterChecked={handlerModal}
                onEdit={onEdit}
                titleTable={"DOCUMENTOS"}
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
              onFinish(e);
            }}
            variant="contained"
            color="primary"
            classes={{ root: classes.buttonMain }}
          >
            Finalizar
          </Button>
        </DialogActions>
      </Dialog>

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
  );
};

export default ReportManualPaymentsModal;
