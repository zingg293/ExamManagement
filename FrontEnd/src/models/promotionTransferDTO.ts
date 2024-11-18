import { WorkflowInstancesDTO } from "@models/workflowInstancesDTO";
import { WorkflowStepDTO } from "@models/workflowStepDTO";

export interface PromotionTransferDTO {
  id: string;
  createdDate: Date;
  status: number | null;
  description: string | null;
  idUserRequest: string;
  idUnit: string;
  unitName: string | null;
  idEmployee: string;
  idPositionEmployeeCurrent: string | null;
  positionNameCurrent: string | null;
  isTransfer: boolean | null;
  isPromotion: boolean | null;
  idCategoryPosition: string | null;
  nameCategoryPosition: string | null;
  isHeadCount: boolean | null;
  workflowInstances: WorkflowInstancesDTO[];
  countWorkFlowStep: number;
  currentWorkFlowStep: WorkflowStepDTO;
}
