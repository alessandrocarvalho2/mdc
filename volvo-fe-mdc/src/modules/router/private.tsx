import React from "react";
import { Route, Redirect } from "react-router-dom";
import AccessService from "../../core/services/access.service";

export default function Private({ component: Component, ...rest }: any) {
  let access = localStorage.getItem("access") || [];
  const accessService = new AccessService();

  if (access.length === 0) {
    console.log("Private");
    accessService.getAccess().then((resp: any) => {
      localStorage.setItem("access", JSON.stringify(resp));
    });
  }

  return (
    <Route
      {...rest}
      exact
      render={(props) =>
        localStorage.getItem("token") ? (
          <Component {...props} />
        ) : (
          <Redirect
            to={{ pathname: "/login", state: { from: props.location } }}
          />
        )
      }
    />
  );
}
