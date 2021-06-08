import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

const feedbackModalStyle = makeStyles((theme: Theme) =>
  createStyles({
    title: {
      textAlign: 'center',
      color: theme.palette.primary.main,
      '& h2': {
        fontSize: '1.2em',
        fontWeight: 'bold',
      }
    },
    text: {
      textAlign: 'center',
      color: '#555555'
    },
    actions: {
      display: 'flex',
      justifyContent: 'center',
      alignContent: 'center',
      paddingBottom: '16px'
    },
    button: {
      '&:hover': {
        background: theme.palette.primary.main,
        color: '#fff'
      }
    }
  }),
);

export default feedbackModalStyle;