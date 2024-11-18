import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { CategoryNationalityDTO } from "@models/CategoryNationalityDTO";

export const CategoryNationalityApisService = createApi({
  reducerPath: "CategoryNationalityApisService",
  tagTypes: ["CategoryNationalityApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryNationality: builder.query<ListResponse<CategoryNationalityDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/categoryNationality/getListCategoryNationality`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryNationalityApisService" as const, id })),
            {
              type: "CategoryNationalityApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryNationalityApisService", id: "LIST" }];
      }
    }),
    UpdateCategoryNationality: builder.mutation<
      payloadResult,
      { CategoryNationality: Partial<CategoryNationalityDTO> }
    >({
      query: ({ CategoryNationality }) => ({
        url: `/categoryNationality/updateCategoryNationality`,
        method: "PUT",
        data: CategoryNationality
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "CategoryNationalityApisService", id: arg.CategoryNationality.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.CategoryNationality && arg.CategoryNationality.id) {
          return [{ type: "CategoryNationalityApisService", id: arg.CategoryNationality.id }];
        }
        return [];
      }
    }),
    GetCategoryNationalityById: builder.query<ListResponse<CategoryNationalityDTO>, { idCategoryNationality: string }>({
      query: ({ idCategoryNationality }): any => ({
        url: `/categoryNationality/getCategorynationalityById`,
        params: { idCategoryNationality }
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryNationalityApisService", id: arg.idCategoryNationality }]
    }),
    InsertCategoryNationality: builder.mutation<
      payloadResult,
      { CategoryNationality: Partial<CategoryNationalityDTO> }
    >({
      query: ({ CategoryNationality }) => ({
        url: `/categoryNationality/insertCategorynationality`,
        method: "POST",
        data: CategoryNationality
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryNationalityApisService", id: "LIST" }] : [])
    }),
    DeleteCategoryNationality: builder.mutation<payloadResult, { idCategoryNationality: string[] }>({
      query: ({ idCategoryNationality }) => ({
        url: `/categoryNationality/DeleteCategoryNationality`,
        method: "DELETE",
        data: idCategoryNationality
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryNationalityApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListCategoryNationalityQuery,
  useUpdateCategoryNationalityMutation,
  useGetCategoryNationalityByIdQuery,
  useInsertCategoryNationalityMutation,
  useDeleteCategoryNationalityMutation
} = CategoryNationalityApisService;
