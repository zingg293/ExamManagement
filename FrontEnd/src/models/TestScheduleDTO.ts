export interface TestScheduleDTO {
  examTypeName: string;
  id: string;
  testScheduleName?: string | null;
  isActive: boolean;
  createdDate?: Date | null;
  idExam?: string | null;
  idExamSubject?: string | null;
  fromDate?: Date | null;
  toDate?: Date | null;
  isDeleted: boolean;
  organizeFinalExams: boolean;
  note?: string | null;
}
