import { UploadFile } from "antd/lib/upload/interface";

export interface NewsDTO {
  id: string;
  title: string;
  description: string;
  isHide: boolean;
  isDeleted: boolean;
  isApproved: boolean;
  userCreated: string;
  userUpdated?: string;
  status: number;
  createdDate: Date;
  createdDateDisplay: Date;
  updateDate?: Date;
  newsContent: string;
  newContentDraft: string;
  author?: string;
  newsLike: number;
  newsView: number;
  avatar: string;
  extensionFile: string;
  filePath: string;
  idCategoryNews: string;

  Files: UploadFile[];
}
