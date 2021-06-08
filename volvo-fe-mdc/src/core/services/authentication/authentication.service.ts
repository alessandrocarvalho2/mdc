import moment from "moment";
import TokenModel from "../../models/token.model";

class AuthenticationService {
  private readonly sessionStorage: string =
    window.location.hostname + "____MDC";

  private isRefreshing = false;

  get sessionData(): TokenModel | null {
    let result = localStorage.getItem(this.sessionStorage);

    if (result) {
      return JSON.parse(result) as TokenModel;
    }
    return null;
  }

  public isAuthenticated() {
    return this.sessionData && this.sessionData.accessToken;
  }

  public logoff() {
    localStorage.clear();
  }

  public getToken() {
    return this.sessionData ? this.sessionData.accessToken : null;
  }

  public getRefreshToken() {
    return this.sessionData ? this.sessionData.refreshToken : null;
  }

  public isAccessTokenExpired() {
    if (this.sessionData && this.sessionData.expiration) {
      return moment().isSameOrAfter(this.sessionData.expiration);
    }

    return false;
  }

}

export default AuthenticationService;
