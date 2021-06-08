import qs from "qs";
import CategoryModel from "../models/category.model";
import api from "./api";
import url from "./url.service";

class CategoryService {
  getAll(): any {
    return api
      .get(url.addr.get.CATEGORY_LIST_ALL)
      .then((resp: any) => resp.data);
  }

  getList(filter?: CategoryModel): any {
    if (filter) {
      const query = qs.stringify({
        bankAccountId: filter.bankAccountId,
        inOut: filter.inOut,
      });
      return api
        .get(url.addr.get.CATEGORY_LIST + `?${query}`)
        .then((resp: any) => resp.data);
    } else
      return api
        .get(url.addr.get.CATEGORY_LIST_ALL)
        .then((resp: any) => resp.data);
  }
}

export default CategoryService;
