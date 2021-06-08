import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";
import LayoutService from "../../../../core/services/layout.service";

const transferBetweenAccountsListStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
    },
    head: {
      // backgroundColor: theme.palette.common.black,
      // color: theme.palette.common.white,
      fontWeight: "bold"
    },
    title: {
      fontSize: "2em",
      fontWeight: 300,
    },
    containertable: {
      maxHeight: LayoutService.getHeightTable(),
    },
    cellTextRed: {
      color: "#C4001A",
      fontWeight: "bold",
    },
    cellTextBlue: {
      color: "#004FBC",
      fontWeight: "bold",
    },
    cellText: {
      color: "black",
    },
    cellTextWhite: {
      color: "white",
    },
    backgroundColorRowMain: {
      backgroundColor: "#B8DeD8",
    },
    backgroundColorRowApply: {
      backgroundColor: "#47962D",
    },
    backgroundColorRowRescue: {
      backgroundColor: "#C4001A",
    },
    button: {
      textTransform: "initial",
    },
    icon: {
      marginRight: "6px",
    },
    paddingSave: {
      paddingTop: "12px",
    },
    buttonText: {
      textTransform: "initial",
    },
    checkBok: {
      alignContent: "center",
      justify: "center",
      alignItems: "center",
    },
    inputText: {
      //display: "flex",
      textAlign: "center",
    },
  })
);

export default transferBetweenAccountsListStyles;
