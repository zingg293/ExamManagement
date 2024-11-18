import { createApi } from "@reduxjs/toolkit/query/react";
import { ListResponse } from "~/models/common";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { CategoryWardDTO } from "@models/categoryWardDTO";

export const CategoryWardApisService = createApi({
  reducerPath: "CategoryWardApisService",
  tagTypes: ["CategoryWardApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryWardByIdDistrict: builder.query<
      ListResponse<CategoryWardDTO>,
      { pageSize: number; pageNumber: number; DistrictCode: string }
    >({
      query: ({
        pageSize,
        pageNumber,
        DistrictCode
      }: {
        pageSize: number;
        pageNumber: number;
        DistrictCode: string;
      }) => ({
        url: `/CategoryWard/getCategoryWardByDistrictCode`,
        method: "GET",
        params: { pageSize, pageNumber, DistrictCode }
      })
    }),
    GetCategoryWardById: builder.query<ListResponse<CategoryWardDTO>, { idCategoryWard: string }>({
      query: ({ idCategoryWard }: { idCategoryWard: string }) => ({
        url: `/CategoryWard/GetCategoryWardById`,
        method: "GET",
        params: { idCategoryWard }
      }),
      keepUnusedDataFor: 0
    })
  })
});

export const {
  useGetListCategoryWardByIdDistrictQuery,
  useGetCategoryWardByIdQuery,
  useLazyGetListCategoryWardByIdDistrictQuery
} = CategoryWardApisService;
