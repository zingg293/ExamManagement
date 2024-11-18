export interface EducationProgramDTO {
  id: string;
  educationProgramName: string;
  status?: number | null;
  isHide?: boolean | null;
  isDeleted: boolean;
}
