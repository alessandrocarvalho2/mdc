import qs from "qs";
import OperationModel from "../models/operation.model";
import api from "./api";
import url from "./url.service";

class OperationService {
  getAll(): any {
    return api.get(url.addr.get.OPERATION_LIST_ALL).then((resp: any) => resp.data);
  }

  getList(filter?: OperationModel): any {
    if (filter) {
      const query = qs.stringify({
        bankAccountId: filter?.bankAccountId,
        inOut: filter?.inOut,
        categoryId: filter?.categoryId,
      });
      return api
        .get(url.addr.get.OPERATION_LIST + `?${query}`)
        .then((resp: any) => resp.data);
    } else
      return api
        .get(url.addr.get.OPERATION_LIST_ALL)
        .then((resp: any) => resp.data);
  }
}

export default OperationService;
