import { createApi } from "@reduxjs/toolkit/query/react";
import { ListResponse } from "~/models/common";
import { CategoryDistrictDTO } from "@models/categoryDistrictDTO";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";

export const CategoryDistrictApisService = createApi({
  reducerPath: "CategoryDistrictApisService",
  tagTypes: ["CategoryDistrictApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryDistrictByIdCity: builder.query<
      ListResponse<CategoryDistrictDTO>,
      { pageSize: number; pageNumber: number; CityCode: string }
    >({
      query: ({ pageSize, pageNumber, CityCode }: { pageSize: number; pageNumber: number; CityCode: string }) => ({
        url: `/categoryDistrict/getCategoryDistrictByCityCode`,
        method: "GET",
        params: { pageSize, pageNumber, CityCode }
      })
    }),
    getCategoryDistrictById: builder.query<ListResponse<CategoryDistrictDTO>, string>({
      query: (idCategoryDistrict: string) => ({
        url: `/categoryDistrict/getCategoryDistrictById`,
        method: "GET",
        params: { idCategoryDistrict }
      })
    })
  })
});
export const {
  useGetListCategoryDistrictByIdCityQuery,
  useLazyGetListCategoryDistrictByIdCityQuery,
  useGetCategoryDistrictByIdQuery
} = CategoryDistrictApisService;
