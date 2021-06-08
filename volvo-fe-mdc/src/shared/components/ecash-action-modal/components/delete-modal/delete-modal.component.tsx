import React from "react";
import Button from "@material-ui/core/Button";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import { deleteModalInterface } from "./delete-modal.interface";
//import { useTranslation } from "react-i18next";
import deleteModalStyle from "./delete-modal.style";

export default function DeleteModal({
  deleteItem,
  handleClickStateModal,
  message
}: deleteModalInterface) {
  //const { t } = useTranslation();
  const classes = deleteModalStyle();

  return (
    <div>
      <DialogTitle
        id="responsive-dialog-title"
        classes={{
          root: classes.title
        }}
      >
        Excluir registro
      </DialogTitle>
      <DialogContent>
        <DialogContentText
          classes={{ root: classes.text }}
          dangerouslySetInnerHTML={{ __html: "DeleteModal" }}
        />
      </DialogContent>
      <DialogActions className={classes.actions}>
        <Button
          variant="outlined"
          color="secondary"
          classes={{ root: classes.button }}
          onClick={() => handleClickStateModal(false)}
        >
          Não
        </Button>
        <Button
          variant="outlined"
          color="secondary"
          classes={{ root: classes.button }}
          onClick={() => deleteItem()}
        >
          Sim
        </Button>
      </DialogActions>
    </div>
  );
}
