export interface ExamSubjectDTO {
  id: string;
  examSubjectName?: string | null;
  isActive: boolean;
  createdDate?: Date | null;
  idExam?: string | null;
  idExamType?: string | null;
  isDeleted: boolean;
}
