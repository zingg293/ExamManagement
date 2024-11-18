export interface WorkflowTemplateDTO {
  id: string;
  workflowName: string | null;
  workflowCode: string;
  order: number;
  startWorkflowButton: string | null;
  defaultCompletedStatus: string | null;
  tableName: string;
  createdDate: Date;
  status: number | null;
}
