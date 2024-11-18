import { axiosClient } from "@apis/api/axiosClient";
import { ListResponse } from "@models/commom";

export const CategoryWardApis = {
  getCategoryWardByDistrictCode: (districtCode: string) :Promise<ListResponse<any>> => {
    return axiosClient.get("/categoryWard/getCategoryWardByDistrictCode", {
      params: {
        districtCode
      }
    });
  },
};