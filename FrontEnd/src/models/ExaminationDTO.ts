export interface ExaminationDTO
{
  id: string;
  examName?: string;
  isActive: boolean;
  createdDate?: Date;
  idExamType?: string;
  schoolYear?: Date;
  isDeleted: boolean;
}