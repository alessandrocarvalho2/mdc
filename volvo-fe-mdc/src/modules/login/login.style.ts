import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

const loginStyles = makeStyles((theme: Theme) =>
  createStyles({
    wrap: {
      backgroundColor: "#E1DFDD"
    },
    mainLogin: {
      backgroundColor: "#E1DFDD",
      height: "100vh"
    },
    login: {
      paddingTop: "100px"
    },
    loginColor: {
      backgroundColor: "red"
    },
    focused: {
      color: "#E1DFDD"
    },
    wrapperLogin: {
      maxWidth: "365px",
      margin: "0 auto"
    },
    wrapperBox: {
      display: "flex",
      alignItems: "center"
    },
    wrapItem: {
      flex: "1"
    },
    wrapperForm: {
      marginTop: "40px",
      width: "100%"
    },
    customInputs: {
      width: "100%",
      marginBottom: "20px"
    },
    inputColor: {
      color: "#E1DFDD",
      fontSize: "16px"
    },
    aElementLogin: {
      color: "#E1DFDD",
      textAlign: "center",
      display: "inherit",
      margin: "30px"
    },
    outraclasse: {
      color: "#E1DFDD",
      fontSize: "16px"
    },
    margin: {
      margin: theme.spacing(1)
    },
    withoutLabel: {
      marginTop: theme.spacing(3)
    },
    textField: {
      overflow: "hidden"
    }
  })
);

export default loginStyles;
