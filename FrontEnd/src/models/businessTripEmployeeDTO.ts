export interface BusinessTripEmployeeDTO {
  id: string;
  createdDate: Date;
  status: number | null;
  idBusinessTrip: string;
  idEmployee: string;
  captain: boolean;
}
