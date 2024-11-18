import { UploadFile } from "antd/lib/upload/interface";
import { WorkflowInstancesDTO } from "@models/workflowInstancesDTO";
import { WorkflowStepDTO } from "@models/workflowStepDTO";

export interface RequestToHiredDTO {
  id: string;
  status: number | null;
  createdDate: Date;
  reason: string | null;
  quantity: number;
  filePath: string | null;
  idCategoryVacancies: string;
  Files: UploadFile[];
  workflowInstances: WorkflowInstancesDTO[];
  countWorkFlowStep: number;
  currentWorkFlowStep: WorkflowStepDTO;
  idUnit: string;
  createdBy: string;
}
