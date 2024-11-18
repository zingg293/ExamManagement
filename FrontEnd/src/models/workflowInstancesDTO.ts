export interface WorkflowInstancesDTO {
  id: string;
  createdDate: Date | null;
  status: number | null;
  workflowName: string | null;
  workflowCode: string | null;
  isCompleted: boolean;
  isTerminated: boolean;
  isDraft: boolean;
  defaultCompletedStatus: string | null;
  templateId: string;
  currentStep: number;
  unitId: string;
  itemId: string;
  createdBy: string;
  requestToChange: boolean;
  message: string | null;
  nameStatus: string | null;
  isApproved: boolean;
}
