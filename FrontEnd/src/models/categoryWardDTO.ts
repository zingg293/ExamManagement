export interface CategoryWardDTO {
  id: string;
  wardName: string | null;
  wardCode: string | null;
  districtCode: string | null;
  status: number | null;
  isHide: boolean | null;
  createdDate: Date | null;
}
