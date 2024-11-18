export interface TicketLaborEquipmentDetailDTO {
  id: string;
  idTicketLaborEquipment: string;
  idCategoryLaborEquipment: string | null;
  quantity: number;
  createdDate: Date;
  status: number | null;
  idEmployee: string | null;
  equipmentCode: string | null;
  isCheck: boolean;
}
