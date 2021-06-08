import { Switch, useRouteMatch } from "react-router-dom";
import React, { Suspense, lazy } from "react";

import { CircularProgress, Grid } from "@material-ui/core";
import settingStyle from "./setting.style";
import RouteAccess from "../../router/route-access";


const DomainList = lazy(
  () => import("./domain/domain-list")
);


export default function SettingRoute() {
  let match = useRouteMatch();
  const classes = settingStyle();

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
            path={`${match.url}/domain-list`}
            exact={true}
            component={() => <DomainList />}
            accessRoute={"DOMAIN_VISUALIZAR"}
          />
        </Switch>
      </Suspense>
    </Grid>
  );
}
