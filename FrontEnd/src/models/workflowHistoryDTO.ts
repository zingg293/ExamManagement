export interface WorkflowHistoryDTO {
  id: string;
  idWorkflowInstance: string;
  idUser: string;
  action: string | null;
  idUnit: string;
  createdDate: Date;
  status: number | null;
  isStepCompleted: boolean;
  comment: string | null;
  message: string | null;
  isCancelled: boolean;
  isRequestToChanged: boolean;
}
