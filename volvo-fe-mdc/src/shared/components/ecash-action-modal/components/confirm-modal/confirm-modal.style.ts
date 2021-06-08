import { makeStyles, createStyles, Theme } from '@material-ui/core/styles';

const confirmModalStyle = makeStyles((theme: Theme) =>
  createStyles({
    title: {
      textAlign: 'center',
      color: theme.palette.secondary.main,
      '& h2': {
        fontSize: '1.2em',
        fontWeight: 'bold',
      }
    },
    icon: {
      marginRight: "6px",
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
      padding: '5px 24px',
      margin: '0 16px',
      '&:hover': {
        background: theme.palette.secondary.main,
        color: '#fff'
      }
    }
  }),
);

export default confirmModalStyle;