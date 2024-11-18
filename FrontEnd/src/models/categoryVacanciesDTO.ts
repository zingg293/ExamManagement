export interface CategoryVacanciesDTO {
  id: string;
  status: number | null;
  createdDate: Date;
  positionName: string | null;
  numExp: number;
  degree: string | null;
  jobRequirements: string | null;
  jobDescription: string | null;
}
