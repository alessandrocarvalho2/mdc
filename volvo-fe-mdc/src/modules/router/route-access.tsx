import React from 'react';
import { Route, Redirect } from 'react-router-dom';

export default function RouteAccess({ exact, component: Component, accessRoute, ...rest }: any) {
  let access: any = localStorage.getItem('access');
  access = JSON.parse(access);

  // console.log("RouteAccess");
 
  // console.log(accessRoute);
  // console.log("access");
  //  console.log(access);

  return (
    <Route
      {...rest}
      exact={exact}
      render={props => (access && access.some((item: string) => item === accessRoute) ? (
        <Component {...props} />
      ) : (
          <Redirect to={{ pathname: '/login', state: { from: props.location } }} />
        ))
      }
    />
  );
}


