export interface PositionEmployeeDTO {
  id: string;
  idEmployee: string;
  idPosition: string;
  isHeadcount: boolean;
  idUnit: string;
  createdDate: Date | null;
  status: number | null;
}
