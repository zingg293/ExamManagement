import { axiosClient } from "@apis/api/axiosClient";
import { ListResponse } from "@models/commom";

export const CategoryCityApis = {
  getListCategoryCityAvailable: ():Promise<ListResponse<any>> => {
    return axiosClient.get("/categoryCity/getListCategoryCityAvailable");
  }
};