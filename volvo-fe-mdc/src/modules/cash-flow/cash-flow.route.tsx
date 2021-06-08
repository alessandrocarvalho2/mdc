import { Route, useRouteMatch } from "react-router-dom";
import React, { lazy } from "react";

const SettingRoute = lazy(
  () => import("./setting/setting.route")
);

const BankStatementRoute = lazy(
  () => import("./bank-statement/bank-statement.route")
);

const BankTransactionRoute = lazy(
  () => import("./bank-transaction/bank-transaction.route")
);

const ReportsRoute = lazy(
  () => import("./reports/reports.route")
);

const Home = lazy(() => import("../home/home"));

export default function CashFlowRoutes() {
  let match = useRouteMatch();
  //useTranslation("admin");

  return (
    <>
      {/* <Route
        path={`${match.path}main-register`}
        component={() => <MainRegisterRoute />}
      /> */}
      <Route
        path={`${match.path}setting`}
        component={() => <SettingRoute />}
      />
      <Route
        path={`${match.path}bank-statement`}
        component={() => <BankStatementRoute />}
      />
      <Route
        path={`${match.path}bank-transaction`}
        component={() => <BankTransactionRoute />}
      />
      <Route
        path={`${match.path}reports`}
        component={() => <ReportsRoute />}
      />

      <Route path={`/home`} component={() => <Home />} />
    </>
  );
}
