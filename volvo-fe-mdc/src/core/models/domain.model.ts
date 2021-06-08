import CategoryModel from "./category.model";
import OperationModel from "./operation.model";

export interface DomainModel {
  id?: number;
  description?: string;
  inOut?: string;
  approvationNeeded?: boolean;
  visible?: boolean;
  categoryId?: number;
  category?: CategoryModel
  operationId?: number;
  operation?: OperationModel
  date?: Date
  isDetailedTransaction?: boolean;
}
export default DomainModel;
