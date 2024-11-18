import { globalVariable } from "~/globalVariable";
import { ListResponse, Pagination } from "@models/commom";
import { axiosClient } from "@apis/api/axiosClient";
import { NewsDTO } from "@models/newsDTO";

export const NewsApis = {
  getListNewsApproved: (params: Pagination): Promise<ListResponse<NewsDTO>> => {
    return axiosClient.get("/News/GetListNewsApproved", { params });
  },
  getNewsById: (idNews: string): Promise<ListResponse<NewsDTO>> => {
    return axiosClient.get(`/News/GetNewsById`, { params: { idNews } });
  },
  increaseNumberView: (idNews: string): Promise<ListResponse<NewsDTO>> => {
    return axiosClient.get(`/News/IncreaseNumberView`, { params: { idNews } });
  },
  getFileImageNews: (fileNameId: string) => {
    return `${globalVariable.urlServerApi}/api/v1/news/getFileImageNews?fileNameId=${fileNameId}`;
  },
  getListNewsByIdCategoryNews: (idCategoryNews: string, params: Pagination): Promise<ListResponse<NewsDTO>> => {
    return axiosClient.get(`/News/GetListNewsByIdCategoryNews`, { params: { idCategoryNews, ...params } });
  },
  searchNews: (params: Pagination, filter: string): Promise<ListResponse<NewsDTO>> => {
    return axiosClient.get(`/News/SearchNews`, { params: { ...params, filter } });
  }
} as const;