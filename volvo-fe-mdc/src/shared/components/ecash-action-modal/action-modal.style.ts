import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

const actionModalStyle = makeStyles((theme: Theme) =>
  createStyles({
    paper: {
      maxWidth: '320px',
    },
    close: {
      padding: '6px',
      display: 'flex',
      width: '100%',
      justifyContent: 'flex-end',
      position: 'absolute'
    }
  }),
);

export default actionModalStyle;