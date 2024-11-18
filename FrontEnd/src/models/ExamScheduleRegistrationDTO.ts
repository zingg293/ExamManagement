export interface ExamScheduleRegistrationDTO {
  id: string;
  isActive: boolean;
  createdDate?: Date | null;
  idEMS?: string | null;
  idStudyGroup?: string | null;
  idDOTS?: string | null;
  numberOfRegistrations?: number | null;
  isDeleted: boolean;
}