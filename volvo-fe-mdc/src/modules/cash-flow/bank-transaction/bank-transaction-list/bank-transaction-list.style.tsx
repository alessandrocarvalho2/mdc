import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";
import LayoutService from "../../../../core/services/layout.service";

const bankTransactionListStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
    },
    rootPaper: {
      width: "142%",
    },
    containertable: {
      maxHeight: LayoutService.getHeightTable(),
      marginTop: "2%",
      //width: "100%",
    },
    heading: {
      fontSize: theme.typography.pxToRem(15),
      fontWeight: theme.typography.fontWeightRegular,
    },
    head: {
      // backgroundColor: theme.palette.common.black,
      // color: theme.palette.common.white,
      fontWeight: "bold"
    },
    cellTextRed: {
      color: "#C4001A",
      fontWeight: "bold"
    },
    cellTextBlue: {
      color: "#004FBC",
      fontWeight: "bold"
    },
    cellText: {
      color: "black",
      //fontWeight: "bold"
    },
    button: {
      textTransform: 'initial'
    },
    icon: {
      marginRight: '6px'
    },
    paddingSave: {
      paddingTop: '12px'
    },
    buttonText: {
      textTransform: 'initial'
    },
    checkBok: {
      alignContent: "center",
      justify: "center",
      alignItems: "center",
    },
    inputText: {
      //display: "flex",
      textAlign: 'center',
    },
    gridPrincipal: {
      border: "1px solid black",
      width: "100%",
      alignContent: "right",
      justify: "flex-end"
    }
  })
);

export default bankTransactionListStyles;
