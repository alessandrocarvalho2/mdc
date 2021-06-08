import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";
import LayoutService from "../../../core/services/layout.service";

const ecashTableHeadStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
    },
    containertable: {
      maxHeight: LayoutService.getHeightTable(),
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
    checkBok: {
      alignContent: "center",
      justify: "center",
      alignItems: "center",
    },
    visuallyHidden: {
      border: 0,
      clip: 'rect(0 0 0 0)',
      height: 1,
      margin: -1,
      overflow: 'hidden',
      padding: 0,
      position: 'absolute',
      top: 20,
      width: 1,
    },
  })
);

export default ecashTableHeadStyles;
