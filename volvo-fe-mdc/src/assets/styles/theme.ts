import { createMuiTheme } from "@material-ui/core/styles";
import { ptBR } from "@material-ui/core/locale";

const theme = createMuiTheme(
  {
    overrides: {
      MuiFormControl: {
        root: {
          width: "100%",
        },
      },
    },
    palette: {
      primary: {
        main: "#182871",
        light: "#5C83AF",
        contrastText: "#fff",
      },
      secondary: {
        main: "#396976",
        light: "#FF5E63",
        contrastText: "#fff",
      },
      text: {
        primary: "rgba(0, 0, 0, 0.87)",
        secondary: "rgba(0, 0, 0, 0.54)",
        disabled: "rgba(0, 0, 0, 0.38)",
        hint: "rgba(0, 0, 0, 0.38)",
      },
    },
  },
  ptBR
);

export default theme;
