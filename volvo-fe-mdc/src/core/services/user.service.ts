import api from "./api";
// import qs from "qs";

class UserService {

  // getUsers(
  //   page: number,
  //   size: number,
  //   search: string
  // ) {
  //   const query = qs.stringify({
  //     size,
  //     page: page + 1,
  //     query: search
  //   });

  //   return api.get(`/usuario?${query}`).then((resp: any) => resp.data);
  // }

  // editUser(payload: any) {
  //   return api.put(`/usuario/${payload.id}`, payload).then((resp: any) => resp.data);
  // }

  // editUserClient(payload: any) {
  //   return api.put(`/usuarioCliente/${payload.id}`, payload).then((resp: any) => resp.data);
  // }

  // newUser(payload: any) {
  //   return api.post('/usuario', payload).then((resp: any) => resp.data);
  // }

  // newUserClient(payload: any) {
  //   return api.post('/usuarioCliente', payload).then((resp: any) => resp.data);
  // }


  // deleteUser(id: number) {
  //   return api.delete(`/usuario/${id}`).then((resp: any) => resp.data.data);
  // }

  // getUser(id: string) {
  //   return api.get(`/usuario/${id}`).then((resp: any) => resp.data.data);
  // }
}

export default UserService;
