import React, { Suspense } from "react";
import { Provider } from "react-redux";
import ReduxToastr from "react-redux-toastr";

import store from "./core/store";
import Routes from './modules/router'
import { BrowserRouter as Router } from "react-router-dom";
import "react-redux-toastr/lib/css/react-redux-toastr.min.css";

const Loader = () => (
  <div className="App">
    <div>loading...</div>
  </div>
);

const App = () => (
  <Provider store={store}>
    <Router>
      <Suspense fallback={<Loader />}>
        <ReduxToastr timeOut={4000} progressBar />
        <Routes></Routes>
      </Suspense>
    </Router>
  </Provider>
);

export default App;
