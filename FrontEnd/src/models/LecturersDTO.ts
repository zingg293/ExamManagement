export interface LecturersDTO {
  id: string;
  fullName: string;
  gender: boolean;
  phone: string;
  status: number;
  createdDate?: Date | null;
  avatar?: string;
  idFaculty: string;
  birthday?: Date | null;
  isDeleted: boolean;
  InstructorCode: string;
}
