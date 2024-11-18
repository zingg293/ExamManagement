import { CategoryCompensationBenefitsDTO } from "@models/categoryCompensationBenefitsDTO";
import { AllowanceDTO } from "@models/allowanceDTO";
import { EmployeeBenefitsDTO } from "@models/employeeBenefitsDTO";
import { UploadFile } from "antd/lib/upload/interface";

export interface EmployeeDTO {
  id: string;
  code: string | null;
  name: string | null;
  sex: boolean | null;
  birthday: Date | null;
  phone: string | null;
  email: string | null;
  address: string | null;
  idCity: string | null;
  idDistrict: string | null;
  idWard: string | null;
  taxNumber: string | null;
  accountNumber: string | null;
  note: string | null;
  avatar: string | null;
  idUnit: string | null;
  status: number | null;
  createdDate: Date;
  salaryBase: number | null;
  socialInsurancePercent: number | null;
  taxPercent: number | null;
  jobGrade: number;
  typeOfEmployee: string | null;
  positionEmployees:
    | {
        id: string;
        idPosition: string;
        idUnit: string;
        isHeadcount: boolean;
      }[]
    | null;

  Files: UploadFile[];

  categoryCompensationBenefitsList: CategoryCompensationBenefitsDTO[] | null;
  allowances: AllowanceDTO[] | null;
  employee: EmployeeDTO | null;
  employeeBenefits: EmployeeBenefitsDTO[] | null;
  idUser: string;
}
