import { Button, Grid } from "@material-ui/core";
import React from "react";
import LogoutService from "../../core/services/logout.service";
import BankAccountSelect from "../../shared/components/ecash-bank-account-select/bank-account.component";

const Home = (props: any) => {
  const logoutService = new LogoutService();

  const logout = () => {
    logoutService.logout();
  };
  const handlerChangeSelect = (name: any, value: any) => {
    props.setSearch({
      ...props.search,
      [name]: value && value.id ? value.id : "",
    });
  };

  return (
    <>
      <h1>Bem vindo!</h1>
      <Grid item md={2}>
        <BankAccountSelect handlerChangeSelect={handlerChangeSelect} />
      </Grid>

      <Button
        variant="contained"
        onClick={logout}
        color="primary"
        href="#contained-buttons"
      >
        Logout
      </Button>
    </>
  );
};

export default Home;
