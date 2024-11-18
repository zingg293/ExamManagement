export interface RollCallDTO {
  id: string;
  idUser: string;
  idLecturer?: string | null;
  idRoom?: string | null;
  idExamShift: string;
  present?: boolean | null;
  note?: string | null;
  createDated?: Date | null;
  isDeleted: boolean;
}