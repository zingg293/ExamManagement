export interface EmployeeBenefitsDTO {
  id: string;
  status: number | null;
  createdDate: Date | null;
  idEmployee: string;
  idCategoryCompensationBenefits: string;
  quantity: number | null;
}
