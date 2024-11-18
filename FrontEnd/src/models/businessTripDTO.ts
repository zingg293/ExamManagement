import { WorkflowInstancesDTO } from "@models/workflowInstancesDTO";
import { WorkflowStepDTO } from "@models/workflowStepDTO";

export interface BusinessTripDTO {
  id: string;
  createdDate: Date;
  status: number | null;
  description: string | null;
  idUserRequest: string;
  idUnit: string;
  unitName: string | null;
  startDate: Date;
  endDate: Date;
  workflowInstances: WorkflowInstancesDTO[];
  countWorkFlowStep: number;
  currentWorkFlowStep: WorkflowStepDTO;
  vehicle: string;
  businessTripLocation: string;
  client: string;
  expense: number;
  attachments: string;
}
