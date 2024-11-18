export interface ExamShiftDTO {
  id: string;
  examShiftName: string;
  timeStart?: Date | null;
  timeEnd?: Date | null;
  isDeleted: boolean;
}