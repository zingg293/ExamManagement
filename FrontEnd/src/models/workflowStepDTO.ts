export interface WorkflowStepDTO {
  id: string;
  templateId: string;
  stepName: string | null;
  idRoleAssign: string;
  idUnitAssign: string | null;
  allowTerminated: boolean;
  rejectName: string | null;
  order: number;
  outCome: string | null;
  createdDate: Date;
  status: number | null;
  processWorkflowButton: string | null;
  statusColor: string | undefined;
  isDirectUnit: boolean;
}
