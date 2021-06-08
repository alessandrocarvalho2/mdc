import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

const spinnerStyles = makeStyles((theme: Theme) =>
  createStyles({
    center: {
      position: "absolute",
      top: "50%",
      left: "50%",
      MozTransformOrigin: "-50% -50% 0",
      WebkitTransformOrigin: "-50% -50% 0",
      transformOrigin: "-50% -50% 0",
      outline: 0,
    },
    overlay: {
      height: "100vh",
      width: "100%",
      backgroundColor: "rgba(136, 139, 141, 0.3)",
      zIndex: 10,
      top: 0,
      left: 0,
      position: "fixed",
    },
    image: {
      position: "absolute",
      left: "50%",
      top: "50%",
      marginLeft: "-50px",
      marginTop: "-50px",
    },
  })
);

export default spinnerStyles;
