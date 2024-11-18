import { WorkflowInstancesDTO } from "@models/workflowInstancesDTO";
import { WorkflowStepDTO } from "@models/workflowStepDTO";

export interface ResignDTO {
  id: string;
  createdDate: Date;
  status: number | null;
  description: string | null;
  idUserRequest: string;
  idEmployee: string;
  idUnit: string;
  unitName: string | null;
  resignForm: string | null;
  workflowInstances: WorkflowInstancesDTO[];
  countWorkFlowStep: number;
  currentWorkFlowStep: WorkflowStepDTO;
}
