export interface DOEMSDTO {
  id: string;
  isActive?: boolean;
  createdDate?: Date | null;
  idEMS: string;
  idLecturer?: string;
  idRoom?: string;
  fromDate?: Date | null;
  toDate?: Date | null;
  idStudyGroup?: string;
  idDOTS?: string;
  isDeleted?: boolean;
  doUseLabRoom?: boolean;
  note?: string | null;
  idTeamcode?: string | null;
}
