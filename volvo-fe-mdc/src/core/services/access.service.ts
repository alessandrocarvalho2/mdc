import api from './api';

class AccessService {

  getAccess() {
    return api.get('/acessos').then((resp: any) => this.formatAccess(resp.data.data));
  }

  formatAccess(items: any) {
    const keys = Object.keys(items);
    let access: string[] = [];

    keys.forEach((key: string) => {
      items[key].forEach((item: string) => {
        access.push(key.concat('_', item))
      })
    });

    return access;
  }
}

export default AccessService;