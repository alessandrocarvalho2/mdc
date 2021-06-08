import { Switch, useRouteMatch } from "react-router-dom";
import React, { Suspense, lazy } from "react";

import { CircularProgress, Grid } from "@material-ui/core";
import reportsStyle from "./reports.style";
import RouteAccess from "../../router/route-access";

const ReportKpi = lazy(
  () => import("./report-kpi/report-kpi")
);

const ReportConciliation= lazy(
  () => import("./report-conciliation/report-conciliation")
);

const ReportOperationalcf= lazy(
  () => import("./report-operationalcf/report-operationalcf")
);


export default function ReportsRoute() {
  let match = useRouteMatch();
  const classes = reportsStyle();

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
            path={`${match.url}/report-kpi`}
            exact={true}
            component={() => <ReportKpi />}
            accessRoute={"REPORTS_VISUALIZAR"}
          />
          <RouteAccess
            path={`${match.url}/report-conciliation`}
            exact={true}
            component={() => <ReportConciliation />}
            accessRoute={"REPORTS_VISUALIZAR"}
          />
          <RouteAccess
            path={`${match.url}/report-operationalcf`}
            exact={true}
            component={() => <ReportOperationalcf />}
            accessRoute={"REPORTS_VISUALIZAR"}
          />
        </Switch>
      </Suspense>
    </Grid>
  );
}
