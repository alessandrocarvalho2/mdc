import { Switch, useRouteMatch } from "react-router-dom";
import React, { Suspense, lazy } from "react";

import { CircularProgress, Grid } from "@material-ui/core";
import bankTransactionStyle from "./bank-transaction.style";
import RouteAccess from "../../router/route-access";

const BankTransactionList = lazy(
  () => import("./bank-transaction-list/bank-transaction-list")
);

const CashConsalidationList = lazy(
  () => import("./cash-consalidation-list/cash-consalidation-list")
);

const TransferBetweenAccountsList = lazy(
  () =>
    import("./transfer-between-accounts-list/transfer-between-accounts-list")
);

export default function BankTransactionRoute() {
  let match = useRouteMatch();
  const classes = bankTransactionStyle();

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
            path={`${match.url}/bank-transaction-list`}
            exact={true}
            component={() => <BankTransactionList />}
            accessRoute={"BANK_TRANSACTION_VISUALIZAR"}
          />
          <RouteAccess
            path={`${match.url}/cash-consalidation-list`}
            exact={true}
            component={() => <CashConsalidationList />}
            accessRoute={"CASH_CONSOLALIDATION_VISUALIZAR"}
          />
          <RouteAccess
            path={`${match.url}/transfer-between-accounts-list`}
            exact={true}
            component={() => <TransferBetweenAccountsList />}
            accessRoute={"TRANSFER-BETWEEN-ACCOUNTS-VISUALIZAR"}
          />
        </Switch>
      </Suspense>
    </Grid>
  );
}
