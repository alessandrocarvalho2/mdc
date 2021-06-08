import { createMuiTheme } from "@material-ui/core/styles";

const resetLogin = createMuiTheme({
  overrides: {
    MuiFormControl: {
      root: {
        "& label.Mui-focused": {
          color: "#000",
        },
      },
    },
    MuiOutlinedInput: {
      input: {
        "&:-webkit-autofill": {
          WebkitBoxShadow: "0 0 0 100px #fff inset",
          WebkitTextFillColor: "#000",
        },
      },
    },
    MuiInput: {
      underline: {
        "&:after": {
          display: "none",
        },
        "&:before": {
          backgroundColor: "#fff",
          height: 1,
        },
        "&:hover:before": {
          borderBottom: "solid 2px #fff!important",
        },
      },
    },
    MuiInputLabel: {
      root: {
        color: "#000",
        "& .Mui-focused": {
          color: "green",
        },
        "&:hover": {
          color: "#E1DFDD",
        },
      },
    },
    MuiSvgIcon: {
      root: {
        color: "#4D4E53", //icone visibilidade
      },
    },
    MuiInputBase: {
      root: {
        color: "#4D4E53", //cor das letras
        "&:$focused": {
          color: "#000", //cor letras selecionado
        },
        "&:hover": {
          color: "#000", //cor letras hover
        },
      },
      input: {
        "&:-webkit-autofill": {
          WebkitBoxShadow: "0 0 0 100px #fff inset",
          WebkitTextFillColor: "#E1DFDD",
          fontSize: "1rem",
        },
        "&:-webkit-autofill:focus": {
          fontSize: "1rem",
        },
      },
    },
    MuiCssBaseline: {
      "@global": {
        body: {
          backgroundColor: "#FFF", //fundo principal
        },
      },
    },
  },
});

export default resetLogin;
