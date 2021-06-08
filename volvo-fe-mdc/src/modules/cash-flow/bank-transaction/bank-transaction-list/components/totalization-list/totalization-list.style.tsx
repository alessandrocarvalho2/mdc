import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

const totalizationListStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
    },
    containertable: {
      maxHeight: "auto",
    },
    head: {
      // backgroundColor: theme.palette.common.black,
      // color: theme.palette.common.white,
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
    checkBok: {
      alignContent: "center",
      justify: "center",
      alignItems: "center",
    },
  })
);

export default totalizationListStyles;
