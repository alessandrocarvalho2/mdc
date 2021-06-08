import React from "react";
import Button from "@material-ui/core/Button";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import { confirmModalInterface } from "./confirm-modal.interface";
import confirmModalStyle from "./confirm-modal.style";
import { Icon } from "@material-ui/core";

export default function ConfirmModal({
  handleClickConfirmModal,
  handleClickStateModal,
  title,
  message,
}: confirmModalInterface) {
  const classes = confirmModalStyle();

  return (
    <div>
      <DialogTitle
        id="responsive-dialog-title"
        classes={{
          root: classes.title,
        }}
      >
        {title}
      </DialogTitle>
      <DialogContent>
        <DialogContentText
          classes={{ root: classes.text }}
          dangerouslySetInnerHTML={{ __html: message }}
        />
      </DialogContent>
      <DialogActions className={classes.actions}>
        <Button
          variant="outlined"
          color="secondary"
          classes={{ root: classes.button }}
          onClick={() => handleClickStateModal(false)}
        >
          <Icon className={classes.icon}>thumb_down</Icon>
          Não
        </Button>
        <Button
          variant="contained"
          color="primary"
          classes={{ root: classes.button }}
          onClick={() => handleClickConfirmModal()}
        >
          <Icon className={classes.icon}>thumb_up</Icon>
          Sim
        </Button>
      </DialogActions>
    </div>
  );
}
