export interface ExaminationTypeDTO {
  id: string;
  examTypeName?: string | null;
  isActive: boolean;
  createdDate?: Date | null;
  isDeleted: boolean;
}
