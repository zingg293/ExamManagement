import { UploadFile } from "antd/lib/upload/interface";

export interface CategoryNewsDTO {
  id: string;
  nameCategory: string;
  categoryGroup: number;
  parentId?: string;
  isHide: boolean;
  isDeleted: boolean;
  userCreated: string;
  showChild: boolean;
  sort: number;
  createdDate: Date;
  status: number;
  avatar: string;

  Files: UploadFile[];
}
