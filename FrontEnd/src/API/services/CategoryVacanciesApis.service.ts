import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { CategoryVacanciesDTO } from "@models/categoryVacanciesDTO";

export const CategoryVacanciesApisService = createApi({
  reducerPath: "CategoryVacanciesApisService",
  tagTypes: ["CategoryVacanciesApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryVacancies: builder.query<ListResponse<CategoryVacanciesDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/categoryVacancies/getListCategoryVacancies`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryVacanciesApisService" as const, id })),
            {
              type: "CategoryVacanciesApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryVacanciesApisService", id: "LIST" }];
      }
    }),
    GetListCategoryVacanciesApproved: builder.query<ListResponse<CategoryVacanciesDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/categoryVacancies/getListCategoryVacanciesApproved`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryVacanciesApisService" as const, id })),
            {
              type: "CategoryVacanciesApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryVacanciesApisService", id: "LIST" }];
      }
    }),
    GetCategoryVacanciesById: builder.query<ListResponse<CategoryVacanciesDTO>, { idCategoryVacancies: string }>({
      query: ({ idCategoryVacancies }): any => ({
        url: `/categoryVacancies/getCategoryVacanciesById`,
        params: { idCategoryVacancies }
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryVacanciesApisService", id: arg.idCategoryVacancies }]
    }),
    InsertCategoryVacancies: builder.mutation<payloadResult, { categoryVacancies: Partial<CategoryVacanciesDTO> }>({
      query: ({ categoryVacancies }) => ({
        url: `/categoryVacancies/insertCategoryVacancies`,
        method: "POST",
        data: categoryVacancies
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryVacanciesApisService", id: "LIST" }] : [])
    }),
    UpdateCategoryVacancies: builder.mutation<payloadResult, { categoryVacancies: Partial<CategoryVacanciesDTO> }>({
      query: ({ categoryVacancies }) => ({
        url: `/categoryVacancies/updateCategoryVacancies`,
        method: "PUT",
        data: categoryVacancies
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              {
                type: "CategoryVacanciesApisService",
                id: arg.categoryVacancies.id
              }
            ]
          : []
    }),
    DeleteCategoryVacancies: builder.mutation<payloadResult, { idCategoryVacancies: string[] }>({
      query: ({ idCategoryVacancies }) => ({
        url: `/categoryVacancies/deleteCategoryVacancies`,
        method: "DELETE",
        data: idCategoryVacancies
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryVacanciesApisService", id: "LIST" }] : [])
    }),
    updateStatusCategoryVacancies: builder.mutation<payloadResult, { idCategoryVacancy: string; status: number }>({
      query: ({ idCategoryVacancy, status }) => ({
        url: `/categoryVacancies/updateStatusCategoryVacancies`,
        method: "PUT",
        params: { idCategoryVacancy, status }
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryVacanciesApisService", id: "LIST" }] : [])
    })
  })
});

export const {
  useGetListCategoryVacanciesQuery,
  useGetCategoryVacanciesByIdQuery,
  useInsertCategoryVacanciesMutation,
  useUpdateCategoryVacanciesMutation,
  useDeleteCategoryVacanciesMutation,
  useUpdateStatusCategoryVacanciesMutation,
  useGetListCategoryVacanciesApprovedQuery
} = CategoryVacanciesApisService;
