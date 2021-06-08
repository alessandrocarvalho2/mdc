import { Switch, useRouteMatch } from "react-router-dom";
import React, { Suspense, lazy } from "react";

import { CircularProgress, Grid } from "@material-ui/core";
import bankStatementStyle from "./bank-statement.style";
import RouteAccess from "../../router/route-access";

const BankStatementList = lazy(
  () => import("./bank-statement-list/bank-statement-list")
);
const CompareList = lazy(() => import("./compare-list/compare-list"));

export default function BankStatementRoute() {
  let match = useRouteMatch();
  const classes = bankStatementStyle();

  return (
    <Grid
      container
      className={classes.root}
      justify="center"
      alignItems="center"
    >
      <Suspense fallback={<CircularProgress />}>
        <Switch>
          <RouteAccess
            path={`${match.url}/bank-statement-list`}
            exact={true}
            component={() => <BankStatementList />}
            accessRoute={"BANK_STATEMENT_VISUALIZAR"}
          />
          <RouteAccess
            path={`${match.url}/compare-list`}
            exact={true}
            component={() => <CompareList />}
            accessRoute={"COMPARE_VISUALIZAR"}
          />
        </Switch>
      </Suspense>
    </Grid>
  );
}
