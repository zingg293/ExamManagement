export interface CategorySalaryLevelDTO {
  id: string;
  createdDate: Date;
  status?: number | null;
  nameSalaryLevel?: string | null;
  amount?: number | null;
  idSalaryScale: string;
  isCoefficient: boolean;
  socialSecurityContributionRate?: number | null;
}