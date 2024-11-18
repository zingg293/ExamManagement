import { axiosClient } from "@apis/api/axiosClient";
import { ListResponse } from "@models/commom";

export const CategoryDistrictApis = {
  getCategoryDistrictByCityCode: (cityCode: string):Promise<ListResponse<any>> => {
    return axiosClient.get("/categoryDistrict/getCategoryDistrictByCityCode", {
      params: {
        cityCode
      }
    });
  }
};