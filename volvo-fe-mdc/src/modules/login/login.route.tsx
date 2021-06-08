import React, { lazy } from "react";
import { Route, useRouteMatch } from "react-router-dom";

const Auth = lazy(() => import("./auth/auth"));

export default function LoginRoutes() {
  const match = useRouteMatch();
  return (
    <>
      <Route path={`${match.url}login`} component={() => <Auth />} />
    </>
  );
}
