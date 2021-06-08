import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

const bankTransactionAddModelStyle = makeStyles((theme: Theme) =>
  createStyles({
    modal: {
      [theme.breakpoints.up("md")]: {
        width: "60%",
      },
    },
    dialogTitle: {
      textAlign: 'center',
      color: theme.palette.primary.main,
      '& h2': {
        fontSize: '1.2em',
        fontWeight: 'bold',
      }
    },
    // {
    //   fontSize: "1em",
    //   color: theme.palette.grey[900],
    //   fontWeight: 500,
    //   margin: "20px 0 0 0",
    // },
    button: {
      padding: "5px 45px",
      "&:hover": {
        background: theme.palette.primary.main,
        color: "#fff",
      },
    },
    buttonUpload: {
      margin: "13px 0 0 0",
      padding: "5px 45px",

      "&:hover": {
        background: theme.palette.primary.main,
        color: "#fff",
      },
    },
    actions: {
      display: "flex",
      justifyContent: "center",
      alignContent: "center",
      paddingBottom: "16px",
    },
    tabs: {
      marginLeft: "23px",
    },
    tab: {
      textTransform: "capitalize",
    },
    text: {
      textAlign: 'center',
      color: '#555555'
    },
    boxButtons: {
      display: "flex",
      justifyContent: "space-between",
      padding: "20px",
    },
    closeButton:{
      padding: '0px',
      display: 'flex',
      width: '100%',
      justifyContent: 'flex-end',
      position: 'absolute'
    },
    inputUpload: {
      display: "none",
    },
    input: {
      display: "flex",
      flexFlow: "column",
      alignItems: "center",
    },
  })
);

export default bankTransactionAddModelStyle;
