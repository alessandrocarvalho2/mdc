import React from 'react';
import Button from '@material-ui/core/Button';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
//import { useTranslation } from 'react-i18next';
import { feedbackModalInterface } from './feedback-modal.interface'
import feedbackModalStyle from './feedback-modal.style';


export default function FeedbackModal({ message, handleClickStateModal }: feedbackModalInterface) {
  //const { t } = useTranslation();
  const classes = feedbackModalStyle();

  return (
    <div>
      <DialogTitle 
        id="responsive-dialog-title"
        classes={{ root: classes.title }}
        >
        Tudo certo!
      </DialogTitle>
      <DialogContent>
        <DialogContentText 
          classes={{ root: classes.text }}
          dangerouslySetInnerHTML={{ __html: "FeedbackModal" }}
        />
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