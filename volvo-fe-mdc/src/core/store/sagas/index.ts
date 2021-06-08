import { all, takeLatest } from "redux-saga/effects";

import { signIn } from "./auth";
import { AuthTypes } from "../ducks/auth";

export default function* rootSaga() {
  // eslint-disable-next-line @typescript-eslint/consistent-type-assertions
  yield all([takeLatest(<any>AuthTypes.SIGN_IN_REQUEST, signIn)]);
}
