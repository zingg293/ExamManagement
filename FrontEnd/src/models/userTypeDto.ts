export interface UserTypeDTO {
  id: string;
  typeName: string;
  status: number | null;
  createBy: string | null;
  createdDate: Date;
  typeCode: string | null;
}
