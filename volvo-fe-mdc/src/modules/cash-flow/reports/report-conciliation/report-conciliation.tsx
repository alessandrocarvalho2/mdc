import React, { useEffect, useState } from "react";
import { Grid, Icon, Button } from "@material-ui/core";
import ReportConciliationStyles from "./report-conciliation.style";
import DatePicker from "../../../../shared/components/ecash-date-picker/date-picker.component";
import ExportService from "../../../../core/services/export.service";
import EcashSnackbar from "../../../../shared/components/ecash-snackbar/ecash-snackbar.component";
import messageService from "../../../../core/services/message.service";
import SnackBarTypeEnum from "../../../../core/enum/snackbar-type.enum";

export default function ReportConciliation(props: any) {
  const classes = ReportConciliationStyles();
  const exportService = new ExportService();

  const [search, setSearch] = useState<any>({
    dateI: new Date(),
  });

  const handlerdDate = (dateStatement: Date | null) => {
    setSearch({
      dateI: dateStatement,
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
    exportService.getExportReportConciliation(search).catch((error) => {

      var enc = new TextDecoder("utf-8");
      
      if (error.response) {

        var arr = new Uint8Array(error.response.data);
        var mensagem = enc.decode(arr);

        if (mensagem !== "") {
          snackBarOpen(mensagem, SnackBarTypeEnum.ERROR);
        } else
          snackBarOpen(
            messageService.error.MSG008.replace("{0}", "conciliação"),
            SnackBarTypeEnum.ERROR
          );
      } else if (error.request) {
        snackBarOpen(
          messageService.error.MSG008.replace("{0}", "conciliação"),
          SnackBarTypeEnum.ERROR
        );
      } else {
        snackBarOpen(
          messageService.error.MSG008.replace("{0}", "conciliação"),
          SnackBarTypeEnum.ERROR
        );
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
            Relatório de resumo da conciliação bancária
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
        <Grid item md={10}>
          <DatePicker
            name="dateI"
            error={errors.date ? true : false}
            helperText={errors.date}
            setSelectedDate={handlerdDate}
            date={search.dateI}
            label="Data inicial"
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
