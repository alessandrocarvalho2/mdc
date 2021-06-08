import React from 'react';
import Button from '@material-ui/core/Button';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
//import { useTranslation } from 'react-i18next';
import { uploadFeedbackInterface } from './upload-feedback-modal.interface'
import useStyle from './upload-feedback-modal.style';


export default function UploadFeedbackModal({ message, handleClickStateModal }: uploadFeedbackInterface) {
  //const { t } = useTranslation();
  const classes = useStyle();

  return (
    <div>
      <DialogTitle 
        id="responsive-dialog-title"
        classes={{ root: classes.title }}
        >
        Atenção
      </DialogTitle>
      <DialogContent>
        <DialogContentText classes={{ root: classes.text }}>
          Quqlquer coisa
          {/* {t(`admin:${message}`)} */}
        </DialogContentText>
      </DialogContent>
      <DialogActions className={ classes.actions }>
        <Button 
          variant="outlined" 
          color="primary"
          classes={{ root: classes.button }}
          onClick={() => handleClickStateModal(false)}
          >
          ok
        </Button>
      </DialogActions>
    </div>
  );
}