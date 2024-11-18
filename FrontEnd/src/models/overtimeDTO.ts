import { WorkflowInstancesDTO } from "@models/workflowInstancesDTO";
import { WorkflowStepDTO } from "@models/workflowStepDTO";

export interface OvertimeDTO {
  id: string;
  createdDate: Date;
  status: number | null;
  description: string | null;
  idUserRequest: string;
  idEmployee: string;
  idUnit: string;
  unitName: string | null;
  workflowInstances: WorkflowInstancesDTO[];
  countWorkFlowStep: number;
  currentWorkFlowStep: WorkflowStepDTO;
  fromDate: Date;
  toDate: Date;
}
