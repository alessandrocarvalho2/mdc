import { makeStyles, createStyles, Theme } from "@material-ui/core/styles";

const mainRegisterStyles = makeStyles((theme: Theme) =>
  createStyles({
    root: {
      [theme.breakpoints.up('md')]: {
        padding: '0 32px 32px 32px'
      }
    }
  }),
);

export default mainRegisterStyles;