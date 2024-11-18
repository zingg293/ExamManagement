import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { CategoryPositionDTO } from "@models/categoryPositionDTO";

export const CategoryPositionApisService = createApi({
  reducerPath: "CategoryPositionApisService",
  tagTypes: ["CategoryPositionApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryPosition: builder.query<ListResponse<CategoryPositionDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/categoryPosition/getListCategoryPosition`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryPositionApisService" as const, id })),
            {
              type: "CategoryPositionApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryPositionApisService", id: "LIST" }];
      }
    }),
    GetListCategoryPositionAvailable: builder.query<ListResponse<CategoryPositionDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/categoryPosition/getListCategoryPositionAvailable`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryPositionApisService" as const, id })),
            {
              type: "CategoryPositionApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryPositionApisService", id: "LIST" }];
      }
    }),
    GetCategoryPositionById: builder.query<ListResponse<CategoryPositionDTO>, { idCategoryPosition: string }>({
      query: ({ idCategoryPosition }): any => ({
        url: `/categoryPosition/getCategoryPositionById`,
        params: { idCategoryPosition }
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryPositionApisService", id: arg.idCategoryPosition }]
    }),
    InsertCategoryPosition: builder.mutation<payloadResult, { categoryPosition: Partial<CategoryPositionDTO> }>({
      query: ({ categoryPosition }) => ({
        url: `/categoryPosition/insertCategoryPosition`,
        method: "POST",
        data: categoryPosition
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryPositionApisService", id: "LIST" }] : [])
    }),
    UpdateCategoryPosition: builder.mutation<payloadResult, { categoryPosition: Partial<CategoryPositionDTO> }>({
      query: ({ categoryPosition }) => ({
        url: `/categoryPosition/updateCategoryPosition`,
        method: "PUT",
        data: categoryPosition
      }),
      invalidatesTags: (result, error, arg) =>
        result ? [{ type: "CategoryPositionApisService", id: arg.categoryPosition.id }] : []
    }),
    DeleteCategoryPosition: builder.mutation<payloadResult, { listId: string[] }>({
      query: ({ listId }) => ({
        url: `/categoryPosition/deleteCategoryPosition`,
        method: "DELETE",
        data: listId
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryPositionApisService", id: "LIST" }] : [])
    }),
    HideCategoryPosition: builder.mutation<payloadResult, { listId: string[]; isHide: boolean }>({
      query: ({ listId, isHide }) => ({
        url: `/categoryPosition/hideCategoryPosition`,
        method: "PUT",
        data: listId,
        params: { isHide }
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryPositionApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListCategoryPositionQuery,
  useGetListCategoryPositionAvailableQuery,
  useGetCategoryPositionByIdQuery,
  useInsertCategoryPositionMutation,
  useUpdateCategoryPositionMutation,
  useDeleteCategoryPositionMutation,
  useHideCategoryPositionMutation
} = CategoryPositionApisService;
