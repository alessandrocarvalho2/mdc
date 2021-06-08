//import api from './api';

class LogoutService {
  logout() {
    localStorage.clear();
    window.location.href = "/login";
    localStorage.clear();
    // return api.post("/logout").then(() => {
    //   //window.location.href = '/login';
    //   //localStorage.clear();
    // });
  }
}

export default LogoutService;
