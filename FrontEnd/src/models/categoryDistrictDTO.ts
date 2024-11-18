export interface CategoryDistrictDTO {
  id: string;
  districtName: string | null;
  districtCode: string | null;
  cityCode: string | null;
  status: number | null;
  isHide: boolean | null;
  createdDate: Date | null;
}
