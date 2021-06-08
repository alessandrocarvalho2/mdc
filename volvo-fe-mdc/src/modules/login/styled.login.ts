import { styled, withStyles } from "@material-ui/styles";

import { Box, TextField, Input, Button } from "@material-ui/core";
import PasswordInput from "../../shared/components/ecash-password-input/password-input.component"

export const Wrap = styled(Box)({
  height: "100vh",
  margin: "0",
  padding: "0",
  maxWidth: "100%"
});

export const WrapLogin = styled(Box)({
  paddingTop: "100px",
  maxWidth: "446px",
  minWidth: "320px",
  margin: "0 auto"
});

export const WrapImg = styled(Box)({
  display: "flex",
  padding: "10px",
  alignItems: "center",
  alignContent: "center",
  justifyContent: "center",
  justifyItems: "center"
});

export const BoxImg = styled(Box)({
  display: "flex",
  margin: "0",
  padding: "10px",
  alignItems: "center",
  alignContent: "center",
  justifyContent: "center"
});

export const WrapForm = styled(Box)({
  marginTop: "30px"
});

export const TextFieldCustom = withStyles({
  root: {
    marginBottom: "30px",
    '& label.Mui-focused': {
      color: '#000',
    },
  },
})(TextField);

export const PasswordInputCustom = withStyles({
  root: {
    marginBottom: "30px",
    '& label.Mui-focused': {
      color: '#000',
    },
  },
})(PasswordInput);

export const InputCustom = styled(Input)({
  marginBottom: "30px"
});

export const ButtonCustom = styled(Button)({
  backgroundColor: "#182871", //cor botao entrar
  width: "100%",
  color: "#fff",
  fontWeight: "bold",
  marginTop: "30px",
  "&:hover": {
    backgroundColor: "#202A44" //hover cor botao entrar
  }
});
