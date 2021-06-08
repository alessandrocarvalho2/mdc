import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

const BankTransactionHeaderStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
      flexGrow: 1,
    },
    title: {
      fontSize: "2em",
      fontWeight: 300,
    },
    icon: {
      marginRight: "6px",
    },
    buttonText: {
      textTransform: "initial",
    },
    filters: {
      paddingBottom: "24px",
    },
    displayHide: {
      display: "none",
    },
    displayShow: {
      display: "show",
    },
    search: {
      height: "72px",
      display: "flex",
      alignItems: "flex-end",
    },
    inputUpload: {
      display: "none",
    }
  })
);

export default BankTransactionHeaderStyles;
