export interface FacultyDTO {
  id: string;
  facultyName: string;
  status?: number | null;
  isHide?: boolean | null;
  isDeleted: boolean;
}
