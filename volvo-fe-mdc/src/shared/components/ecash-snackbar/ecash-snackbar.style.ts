import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

const ecashSnackbarStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      width: "100%",
      "& > * + *": {
        marginTop: theme.spacing(2),
      },
    },
  })
);

export default ecashSnackbarStyles;
