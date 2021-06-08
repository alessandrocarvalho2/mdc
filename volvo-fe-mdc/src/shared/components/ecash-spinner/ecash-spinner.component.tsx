import { CircularProgress } from "@material-ui/core";
import React from "react";
import spinnerStyles from "./ecash-spinner.style";

const Spinner = () => {
  const classes = spinnerStyles();
  return (
    <div className={classes.center}>
      <div className={classes.overlay}> </div>
      <div className={classes.image}>
        <CircularProgress />
      </div>
    </div>
  );
};

export default Spinner;
