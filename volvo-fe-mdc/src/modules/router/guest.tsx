import React from 'react';
import { Route, Redirect } from 'react-router-dom';


export default function Guest({ component: Component, ...rest }: any) {

  return (
    <Route
      {...rest}
      exact
      render={props => (!localStorage.getItem('token') ? (
        <Component {...props} />
      ) : (
          <Redirect to={{ pathname: '/cash-flow', state: { from: props.location } }} />
        ))
      }
    />  
  );
}


