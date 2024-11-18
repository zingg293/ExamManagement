import { createApi } from "@reduxjs/toolkit/query/react";
import { ListResponse } from "~/models/common";
import { CategoryCityDTO } from "@models/categoryCityDTO";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";

export const CategoryCityApisService = createApi({
  reducerPath: "CategoryCityApisService",
  tagTypes: ["CategoryCityApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryCity: builder.query<ListResponse<CategoryCityDTO>, { pageSize: number; pageNumber: number }>({
      query: ({ pageSize, pageNumber }: { pageSize: number; pageNumber: number }) => ({
        url: `/CategoryCity/GetListCategoryCity`,
        method: "GET",
        params: { pageSize, pageNumber }
      }),
      keepUnusedDataFor: 60 * 60
    }),
    GetListCategoryCityAvailable: builder.query<
      ListResponse<CategoryCityDTO>,
      {
        pageSize: number;
        pageNumber: number;
      }
    >({
      query: ({ pageSize, pageNumber }: { pageSize: number; pageNumber: number }) => ({
        url: `/CategoryCity/getListCategoryCityAvailable`,
        method: "GET",
        params: { pageSize, pageNumber }
      }),
      keepUnusedDataFor: 60 * 60
    }),
    GetCategoryCityById: builder.query<ListResponse<CategoryCityDTO>, { idCategoryCity: string }>({
      query: ({ idCategoryCity }: { idCategoryCity: string }) => ({
        url: `/CategoryCity/GetCategoryCityById`,
        method: "GET",
        params: { idCategoryCity }
      }),
      keepUnusedDataFor: 0
    })
  })
});

export const { useGetListCategoryCityQuery, useGetCategoryCityByIdQuery, useGetListCategoryCityAvailableQuery } =
  CategoryCityApisService;
