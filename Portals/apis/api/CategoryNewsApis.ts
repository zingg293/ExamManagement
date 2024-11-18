import { ListResponse, Pagination } from "@models/commom";
import { axiosClient } from "@apis/api/axiosClient";
import { CategoryNewsDTO } from "@models/categoryNewsDTO";
import { globalVariable } from "~/globalVariable";

export const CategoryNewsApis = {
  getListCategoryNewsAvailable: (params: Pagination): Promise<ListResponse<CategoryNewsDTO>> => {
    return axiosClient.get("/CategoryNews/getListCategoryNewsAvailable", { params });
  },
  getListCategoryNewsByIdParent: (idParent: string): Promise<ListResponse<CategoryNewsDTO>> => {
    return axiosClient.get(`/CategoryNews/GetListCategoryNewsByIdParent`, { params: { idParent } });
  },
  getCategoryNewsById: (idCategoryNews: string): Promise<ListResponse<CategoryNewsDTO>> => {
    return axiosClient.get(`/CategoryNews/GetCategoryNewsById`, { params: { idCategoryNews } });
  },
  getFileImage: (fileName: string) => {
    return `${globalVariable.urlServerApi}/api/v1/categoryNews/getFileImage?fileNameId=${fileName}`;
  }
} as const;