import { call, put } from "redux-saga/effects";
import { actions as toastrActions } from "react-redux-toastr";
import api from "../../services/api";
import { createBrowserHistory } from "history";
import AuthActions from "../ducks/auth";
import urlService from "../../services/url.service";

interface AccessData {
  username: string;
  password: string;
}

export function* signIn({
  username,
  password,
}: AccessData): Generator<any, void, any> {
  try {
    let response: any = {};
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    response = yield call(api.post, urlService.addr.post.LOGIN, {
      login: username,
      password: password,
    });

    //console.log(response.data);

    //const keys = Object.keys(response.data.data.acessos);
    let keys: any[] = [];

    keys.push("cash-flow");
    keys.push("bank-statement");
    keys.push("bank-transaction");
    keys.push("reports");
    //keys.push("settings");

    let access: string[] = [];

    access.push("home");
    access.push("cash-flow");
    access.push("bank-statement-list");
    access.push("bank-transaction-list");
    access.push("cash-consalidation-list");
    access.push("transfer-between-accounts-list");
    access.push("compare-list");
    access.push("reports");
    access.push("report-kpi");
    access.push("report-conciliation");
    access.push("report-operationalcf");
    //access.push("domain-list");
    access.push("BANK_STATEMENT_VISUALIZAR");
    access.push("BANK_TRANSACTION_VISUALIZAR");
    access.push("CASH_CONSOLALIDATION_VISUALIZAR");
    access.push("TRANSFER-BETWEEN-ACCOUNTS-VISUALIZAR");
    access.push("COMPARE_VISUALIZAR");
    access.push("REPORTS_VISUALIZAR");
    //access.push("SETTINGS_VISUALIZAR");
    //access.push("DOMAIN_VISUALIZAR");

    

    //access.push("cash-flow/bank-statement");

    keys.forEach((key: string) => {
      keys.forEach((item: string) => {
        //access.push(key.concat("_", item));
      });
    });

    //localStorage.setItem("avatar", response.data.data.avatar);

    localStorage.setItem("access", JSON.stringify(access));

    const tokenSuccess = response.data.token;

    const history = createBrowserHistory();

    console.log(tokenSuccess);

    if (tokenSuccess !== undefined) {
      localStorage.setItem("token", tokenSuccess);
      yield put(
        toastrActions.add({
          type: "success",
          title: "Tudo certo!",
          message: "Entrando...",
        })
      );

      history.push("./cash-flow");
      window.location.href = "/cash-flow";

      // history.push("./admin");
      // window.location.href = "/admin";
    } else {
      yield put(
        toastrActions.add({
          type: "error",
          title: "Erro na autenticação",
          message: "Verifique seu usuário ou senha",
        })
      );
    }

    yield put(AuthActions.signInSuccess(tokenSuccess));
  } catch (err) {
    console.log(err);
    if (err.response.data) {
      yield put(
        toastrActions.add({
          type: "error",
          title: "Erro na autenticação",
          message: err.response.data,
        })
      );
    } else {
      yield put(
        toastrActions.add({
          type: "error",
          title: "Erro na autenticação",
          message: "Verifique seu usuário ou senha",
        })
      );
    }
  }
}
