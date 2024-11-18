import { axiosClient } from "@apis/api/axiosClient";
import { globalVariable } from "~/globalVariable";
import { ListResponse } from "@models/commom";
import { CompanyInformationDTO } from "@models/companyInformationDTO";

export const CompanyInformationApis = {
  getListCompanyInformation: (): Promise<ListResponse<CompanyInformationDTO>> => {
    return axiosClient.get("/CompanyInformation/GetListCompanyInformation");
  },
  getFileImage: (fileNameId: string) => {
    return `${globalVariable.urlServerApi}/api/v1/CompanyInformation/getFileImage?fileNameId=${fileNameId}`;
  }
} as const;