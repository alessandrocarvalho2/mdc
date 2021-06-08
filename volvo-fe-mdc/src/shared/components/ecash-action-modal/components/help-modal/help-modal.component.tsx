import React from 'react';
import Button from '@material-ui/core/Button';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
//import { useTranslation } from 'react-i18next';
import { helpModalInterface } from './help-modal.interface'
import helpModalStyle from './help-modal.style';


export default function HelpModal({ message, handleClickStateModal }: helpModalInterface) {
  //const { t } = useTranslation();
  const classes = helpModalStyle();

  return (
    <div>
      <DialogTitle 
        id="responsive-dialog-title"
        classes={{ root: classes.title }}
        >
        Atenção
      </DialogTitle>
      <DialogContent>
        <DialogContentText
          classes={{ root: classes.text }}
          dangerouslySetInnerHTML={{ __html: "HelpModal" }}
        />
      </DialogContent>
      <DialogActions className={ classes.actions }>
        <Button 
          variant="outlined" 
          color="primary"
          classes={{ root: classes.button }}
          onClick={() => handleClickStateModal(false)}
          >
          Solicitar Ajuda
        </Button>
      </DialogActions>
    </div>
  );
}