export interface EmployeeDayOffDTO {
  id: string;
  idEmployee: string;
  dayOff: Date | null;
  typeOfDayOff: number | null;
  createdDate: Date;
  status: number | null;
  onLeave: number | null;
}
