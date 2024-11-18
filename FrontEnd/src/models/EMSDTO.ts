export interface EMSDTO {
  id: string;
  EMSName?: string | null;
  isActive: boolean;
  createdDate?: Date | null;
  idExam: string;
  idExamSubject: string;
  idTestSchedule: string;
  numberOfStudents?: number | null;
  numberOfLecturers?: number | null;
  idStudyGroup?: string;
  isDeleted: boolean;
}
