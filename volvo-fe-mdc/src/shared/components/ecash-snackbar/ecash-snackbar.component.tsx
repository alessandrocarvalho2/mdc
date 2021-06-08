import React, { useEffect, useState } from "react";
import Snackbar from "@material-ui/core/Snackbar";
import MuiAlert, { AlertProps } from "@material-ui/lab/Alert";
import ecashSnackbarStyles from "./ecash-snackbar.style";
import { SnackBarTypeEnum } from "../../../core/enum/snackbar-type.enum";

interface EcashSnackbarInterface {
  open: any;
  setOpen: any;
  type: SnackBarTypeEnum;
  message: any;
}

const EcashSnackbar = (props: EcashSnackbarInterface) => {
  const classes = ecashSnackbarStyles();
  const { open, type, message, setOpen } = props;

  function Alert(props: AlertProps) {
    return <MuiAlert elevation={6} variant="filled" {...props} />;
  }

  const handleClose = (event?: React.SyntheticEvent, reason?: string) => {
    if (reason === "clickaway") {
      return;
    }
    setOpen(false);
  };

  const severityColor: any = "info";

  const [severity, setSeverity] = useState(severityColor);

  useEffect(() => {
    switch (type) {
      case SnackBarTypeEnum.SUCCESS:
        setSeverity("success");
        break;
      case SnackBarTypeEnum.WARNING:
        setSeverity("warning");
        break;
      case SnackBarTypeEnum.ERROR:
        setSeverity("error");
        break;
      default:
        setSeverity("info");
        break;
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [type]);

  return (
    <div className={classes.root}>
      <Snackbar open={open} autoHideDuration={6000} onClose={handleClose}>
        <Alert onClose={handleClose} severity={severity}>
          {message}
        </Alert>
      </Snackbar>
    </div>
  );
};

export default EcashSnackbar;
