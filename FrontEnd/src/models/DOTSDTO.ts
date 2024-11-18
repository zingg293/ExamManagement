export interface DOTSDTO {
  id: string;
  idExam?: string | null;
  idExamSubject?: string | null;
  idTestSchedule?: string | null;
  idExamForm?: string | null;
  examTime?: number | null;
  isDeleted: boolean;
}
