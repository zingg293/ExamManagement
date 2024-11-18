export interface TrainingSystemDTO {
  id: string;
  trainingSystemName: string;
  status?: number | null;
  isHide?: boolean | null;
  idEduProgram?: string;
  isDeleted: boolean;
}
