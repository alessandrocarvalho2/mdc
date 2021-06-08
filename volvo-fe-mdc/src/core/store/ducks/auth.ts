import { createReducer, createActions } from "reduxsauce";
import Immutable from "seamless-immutable";

const { Types, Creators } = createActions({
  signInRequest: ["username", "password"],
  signInSuccess: ["token"],
});

export const AuthTypes = Types;
export default Creators;

export const INITIAL_STATE = Immutable({
  signedIn: false,
  token: null,
});

export const success = (
  state: { merge: (arg0: { signedIn: boolean; token: any }) => any },
  { token }: any
) => {
  return state.merge({ signedIn: true, token });
};

export const reducer = createReducer(INITIAL_STATE, {
  [Types.SIGN_IN_SUCCESS as string]: success,
});
