export interface ExamDutyRegistrationDetailsDTO {
  id: string;
  idExamScheduleRegistration: string;
  idLecturer?: string | null;
  idExamShift?: string | null;
  isDeleted: boolean;
}