import axios from "axios";
import {BASE_URL} from './environment';

const AuthInterceptor = axios.create({
  baseURL: BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

AuthInterceptor.interceptors.request.use(
  async (config: any) => {
    if (!config.url.endsWith("login")) {
      const token = localStorage.getItem("token");
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }

      return config;
    }

    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

AuthInterceptor.interceptors.response.use(
  (response) => {
    // console.log("response");
    // console.log(response);
    if (response.status === 401) {
      alert("You are not authorized");
      localStorage.clear();
      window.location.href = "/login";
    } else if (response.status === 403) {
      alert("You are not authorized");
      localStorage.clear();
      window.location.href = "/login";
    }

    return response;
  },
  (error) => {
    if (error?.response && error.response.status === 403) {
      localStorage.clear();
      window.location.href = "/login";
    }

    return Promise.reject(error);
  }
);

export default AuthInterceptor;
