import { UploadFile } from "antd/lib/upload/interface";

export interface CompanyInformationDTO {
  id: string;
  companyName: string;
  taxNumber: string;
  accountNumber: string;
  address: string;
  phoneNumber: string;
  email: string;
  openingHours: string;
  createdDate: Date;
  status: number;
  copyright: string;
  logo: string;
  fax: string;

  Files: UploadFile[];
}
