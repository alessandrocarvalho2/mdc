import React, { Suspense } from "react";
import { Route, Switch } from "react-router-dom";
import CashFlow from "../cash-flow/cash-flow";
// eslint-disable-next-line
import Home from "../home/home";
import Login from "../login/auth/auth";
// import EcommerceRoutes from "../ecommerce/ecommerce.route";
// import Admin from "../admin/admin";
 import Guest from "./guest";
import Private from "./private";

const Routes = () => (
  <Suspense fallback>
    <Switch>
      {/* <Route exact path="/bank-statement-list" component={BankStatementList} /> */}
      {/* <Guest path="/login" component={Login}></Guest> */}
      <Guest path="/login" component={Login} />
      <Private  path="/cash-flow/" component={CashFlow}></Private>
      {/* <Private  path="/cash-flow/bank-statement-list" component={BankStatementList}></Private> */}
      {/* <Route path="/login" component={Login} /> */}
      {/* <Guest path="/login" component={Login}></Guest> */}

      {/* <Private path="/admin/" component={Admin}></Private> */}
      <Route path="/" component={Login} />
    </Switch>
  </Suspense>
);

export default Routes;
