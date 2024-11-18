export interface UnitDTO {
  id: string;
  unitName: string;
  parentId: string | null;
  status: number | null;
  createdBy: string | null;
  createdDate: Date;
  unitCode: string | null;
  isHide: boolean | null;
  children: UnitDTO[];
}
