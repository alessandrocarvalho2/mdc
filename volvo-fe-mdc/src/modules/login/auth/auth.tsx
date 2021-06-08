import React, { Component } from "react";
import { connect } from "react-redux";
import { bindActionCreators } from "redux";
import AuthActions from "../../../core/store/ducks/auth";
import { CssBaseline, FormControl } from "@material-ui/core";
import { ThemeProvider } from "@material-ui/styles";
import resetLogin from "../reset.login.style";

import {
  Wrap,
  WrapLogin,
  WrapImg,
  BoxImg,
  WrapForm,
  TextFieldCustom,
  ButtonCustom,
  PasswordInputCustom,
} from "../styled.login";
import VolvoLogo from "../../../shared/components/ecash-images/volvo-logo";

//import { signIn } from "../../../core/store/sagas/auth";

// import PasswordInput from "../../../shared/components/ecash-compopnent-input/password-input.component";

class SignIn extends Component {
  state = {
    username: "",
    password: "",
  };

  handleSubmit = (e: { preventDefault: () => void }) => {
    e.preventDefault();

    const { username, password } = this.state;
    const { signInRequest }: any = this.props;

    signInRequest(username, password);
  };

  handleInputChange = (e: { target: { name: any; value: any } }) => {
    if (e.target.name === "username") {
      e.target.value = e.target.value.toString().toUpperCase();
      if (e.target.value.length > 7) {
        e.target.value = e.target.value.toString().slice(0, 8);
      }
    }

    // if (e.target.name === "password") {
    //   if (e.target.value.length > 8) {
    //     e.target.value = e.target.value.toString().slice(0,8)
    //   }
    // }

    this.setState({ [e.target.name]: e.target.value });
  };

  render() {
    const { username, password } = this.state;
    const focused = {
      color: "#fff",
    };

    return (
      <>
        <Wrap m={3}>
          <ThemeProvider theme={resetLogin}>
            <CssBaseline />
            <WrapLogin m={3}>
              <WrapImg m={3}>
              <VolvoLogo width="100" height="100%"/>
                <BoxImg m={3}>
                  <VolvoLogo width="100px" height="100px"/>
                </BoxImg>
              </WrapImg>
              <WrapForm m={3}>
                <form onSubmit={this.handleSubmit}>
                  <FormControl fullWidth>
                    <TextFieldCustom
                      type="text"
                      variant="outlined"
                      name="username"
                      label="Login"
                      value={username}
                      onChange={this.handleInputChange}
                      required
                    />
                  </FormControl>
                  <FormControl fullWidth>
                    <PasswordInputCustom
                      password={password}
                      name="password"
                      class={focused}
                      handlerChange={this.handleInputChange}
                    />
                  </FormControl>
                  <ButtonCustom
                    variant="contained"
                    type="submit"
                    className="btEnter"
                  >
                    Entrar
                  </ButtonCustom>
                </form>
              </WrapForm>
            </WrapLogin>
          </ThemeProvider>
        </Wrap>
      </>
    );
  }
}

const mapDispatchToProps = (dispatch: any) =>
  bindActionCreators(AuthActions, dispatch);
export default connect(null, mapDispatchToProps)(SignIn);
