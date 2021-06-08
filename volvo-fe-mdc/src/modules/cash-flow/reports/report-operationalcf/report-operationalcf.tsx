import React, { useEffect, useState } from "react";
import { Grid, Icon, Button } from "@material-ui/core";
import ReportOperacionalcfStyles from "./report-operationalcf.style";
import DatePicker from "../../../../shared/components/ecash-date-picker/date-picker.component";
import ExportService from "../../../../core/services/export.service";
import EcashSnackbar from "../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import SnackBarTypeEnum from "../../../../core/enum/snackbar-type.enum";
import messageService from "../../../../core/services/message.service";

export default function ReportOperacionalcf(props: any) {
  const classes = ReportOperacionalcfStyles();
  const exportService = new ExportService();

  const [search, setSearch] = useState<any>({
    dateI: new Date(),
    dateF: new Date(),
  });

  const handlerdDateI = (dateStatement: Date | null) => {
    setSearch({
      dateI: dateStatement,
      dateF: search.dateF,
    });
  };

  const handlerdDateF = (dateStatement: Date | null) => {
    setSearch({
      dateI: search.dateI,
      dateF: dateStatement,
    });
  };

  const [snackOpen, setSnackOpen] = useState(false);
  const [typeSnack, setTypeSnack] = useState(SnackBarTypeEnum.INFO);
  const [message, setMessage] = useState("");

  const snackBarOpen = (message: string, type: SnackBarTypeEnum) => {
    setSnackOpen(true);
    setMessage(message);
    setTypeSnack(type);
  };

  const onSubmit = () => {
    exportService.getExportReportOperacionalcf(search).catch((error) => {

      if (error.response) {

        var enc = new TextDecoder("utf-8");
        var arr = new Uint8Array(error.response.data);
        var mensagem = enc.decode(arr);

        if (mensagem !== "") {
          snackBarOpen(mensagem, SnackBarTypeEnum.ERROR);
        } else
          snackBarOpen(
              messageService.error.MSG008.replace("{0}", "consolidado caixa"),
            SnackBarTypeEnum.ERROR
          );
      } else if (error.request) {
        snackBarOpen(
          messageService.error.MSG008.replace("{0}", "consolidado caixa"),
          SnackBarTypeEnum.ERROR
        );
      } else {
        snackBarOpen(
          messageService.error.MSG008.replace("{0}", "consolidado caixa"),
          SnackBarTypeEnum.ERROR
        );
        // Something happened in setting up the request and triggered an Error
        console.log("Error", error.message);
      }
    });
  };

  const initialErros = {
    date: "",
  };

  const [errors, setErrors] = useState(initialErros);

  const clearErrors = () => {
    let temp = { ...errors };

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
  }, [props.search?.date]);

  return (
    <>
      <Grid container spacing={2} alignItems="center" justify="space-between">
        <Grid item md={6}>
          <span className={classes.title}>
            Relatório de fluxo de caixa realizado
          </span>
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
          <DatePicker
            name="dateI"
            error={errors.date ? true : false}
            helperText={errors.date}
            setSelectedDate={handlerdDateI}
            date={search.dateI}
            label="Data inicial"
          />
        </Grid>
        <Grid item md={5}>
          <DatePicker
            name="dateF"
            error={errors.date ? true : false}
            helperText={errors.date}
            setSelectedDate={handlerdDateF}
            date={search.dateF}
            label="Data final"
          />
        </Grid>
        <Grid item md={2}>
          <Button
            disableElevation
            fullWidth
            variant="contained"
            color="secondary"
            classes={{ label: "" }}
            onClick={() => onSubmit()}
          >
            <Icon className="SaveAlt">publish</Icon>
            Exportar
          </Button>
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
