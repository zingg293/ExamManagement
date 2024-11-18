import { TicketLaborEquipmentDetailDTO } from "@models/ticketLaborEquipmentDetailDTO";
import { UploadFile } from "antd/lib/upload/interface";
import { WorkflowInstancesDTO } from "@models/workflowInstancesDTO";
import { WorkflowStepDTO } from "@models/workflowStepDTO";

export interface TicketLaborEquipmentDTO {
  id: string;
  idUserRequest: string;
  type: number;
  reason: string | null;
  fileAttachment: string | null;
  description: string | null;
  createdDate: Date;
  status: number | null;
  idUnit: string;
  ticketLaborEquipmentDetail: TicketLaborEquipmentDetailDTO[];
  Files: UploadFile[];
  workflowInstances: WorkflowInstancesDTO[];
  countWorkFlowStep: number;
  currentWorkFlowStep: WorkflowStepDTO;
}
