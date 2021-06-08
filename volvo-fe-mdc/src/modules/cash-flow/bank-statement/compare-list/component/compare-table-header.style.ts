import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

const compareHeaderStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
    },
    title: {
      fontSize: "2em",
      fontWeight: 300,
    },
    icon: {
      marginRight: "6px",
    },

    center: {
      //marginRight: "6px",
      display: "flex",
      alignItems: "center",
    },
    buttonText: {
      textTransform: "initial",
    },
    iconReverse: {
      textTransform: "initial",
      //MozTransform: "scaleX(-1)",
      //OTransform: "scaleX(-1)",
      //WebkitTransform: "scaleX(-1)",
      transform: "scaleX(-1)",
    },
    filters: {
      paddingBottom: "24px",
    },
    iconOnly: {
      // marginRight: "6px",
    },
    search: {
      height: "72px",
      display: "flex",
      alignItems: "flex-end",
    },
    displayHide: {
      display: "none",
    },
    displayShow: {
      display: "show",
      paddingBottom: "24px"
    },
  })
);

export default compareHeaderStyles;
