import React from "react";
import { Dialog, IconButton } from "@material-ui/core";
import actionModalStyle from "./action-modal.style";
import { Close } from "@material-ui/icons";
import DeleteModal from "./components/delete-modal/delete-modal.component";
import FeedbackModal from "./components/feedback-modal/feedback-modal.component";
import UploadFeedbackModal from "./components/upload-feedback-modal/upload-feedback-modal.component";
import HelpModal from "./components/help-modal/help-modal.component";
import ConfirmModal from "./components/confirm-modal/confirm-modal.component";
import ModalTypeEnum from "../../../core/enum/modal-type.enum";

export default function ActionModal(props: any) {
  const classes = actionModalStyle();
   

  return (
    <div>
      <Dialog
        open={props.open}
        onClose={() => props.handleClickStateModal(false)}
        aria-labelledby="responsive-dialog-title"
        classes={{
          paper: classes.paper,
        }}
      >
        <div className={classes.close}>
          <IconButton
            size="small"
            aria-label="delete"
            onClick={() => props.handleClickStateModal(false)}
          >
            <Close />
          </IconButton>
        </div>
        {props.typeModal === ModalTypeEnum.FEED_BACK && (
          <FeedbackModal
            message={props.messageModal}
            handleClickStateModal={props.handleClickStateModal}
          />
        )}
        {props.typeModal === ModalTypeEnum.HELP && (
          <HelpModal
            message={props.messageModal}
            handleClickStateModal={props.handleClickStateModal}
          />
        )}
        {props.typeModal === ModalTypeEnum.DELETE && (
          <DeleteModal
            message={props.messageModal}
            handleClickStateModal={props.handleClickStateModal}
            deleteItem={props.deleteItem}
          />
        )}
        {props.typeModal === ModalTypeEnum.UPLOAD && (
          <UploadFeedbackModal
            message={props.messageModal}
            handleClickStateModal={props.handleClickStateModal}
          />
        )}
        {props.typeModal === ModalTypeEnum.CONFIRM && (
          <ConfirmModal
            message={props.messageModal}
            title={props.titleModal}
            handleClickStateModal={props.handleClickStateModal}
            handleClickConfirmModal={props.handleClickConfirmModal}
          />
        )}
      </Dialog>
    </div>
  );
}
