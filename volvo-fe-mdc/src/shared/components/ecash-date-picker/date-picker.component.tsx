import "date-fns";
import DateFnsUtils from "@date-io/date-fns";
import React from "react";
import {
  MuiPickersUtilsProvider,
  KeyboardDatePicker,
} from "@material-ui/pickers";

export default function DatePicker(props: any) {
  const handleDateChange = (date: Date | null) => {
    props.setSelectedDate(date);
  };

  return (
    <MuiPickersUtilsProvider utils={DateFnsUtils}>
      <KeyboardDatePicker
        disabled={props.disabled}
        disableFuture={props.disableFuture}
        disablePast={props.disablePast}
        disableToolbar
        error={props.error}
        helperText={props.helperText}
        autoOk={true}
        variant="inline"
        format="dd/MM/yyyy"
        //margin="normal"
        name="date"
        label={props.label}
        value={props.date}
        onChange={handleDateChange}
        KeyboardButtonProps={{
          "aria-label": "change date",
        }}
        invalidDateMessage={"Data inválida"}
        minDateMessage={"A data não deve ser anterior à data mínima"}
      />
    </MuiPickersUtilsProvider>
  );
}
