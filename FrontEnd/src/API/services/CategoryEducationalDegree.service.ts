import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import {ListResponse, pagination, payloadResult} from "@models/common";
import {CategoryEducationalDegreeDTO} from "@models/CategoryEducationalDegreeDTO";

export const CategoryEducationalDegreeApisService = createApi({
  reducerPath: "CategoryEducationalDegreeApisService",
  tagTypes: ["CategoryEducationalDegreeApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryEducationalDegree: builder.query<ListResponse<CategoryEducationalDegreeDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/CategoryEducationalDegree/getListCategoryEducationalDegree`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryEducationalDegreeApisService" as const, id })),
            {
              type: "CategoryEducationalDegreeApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryEducationalDegreeApisService", id: "LIST" }];
      }
    }),
    UpdateCategoryEducationalDegree: builder.mutation<payloadResult, { CategoryEducationalDegree: Partial<CategoryEducationalDegreeDTO> }>({
      query: ({ CategoryEducationalDegree }) => ({
        url: `/CategoryEducationalDegree/updateCategoryEducationalDegree`,
        method: "PUT",
        data: CategoryEducationalDegree
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "CategoryEducationalDegreeApisService", id: arg.CategoryEducationalDegree.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.CategoryEducationalDegree && arg.CategoryEducationalDegree.id) {
          return [{ type: "CategoryEducationalDegreeApisService", id: arg.CategoryEducationalDegree.id }];
        }
        return [];
      }
    }),
    GetCategoryEducationalDegreeById: builder.query<ListResponse<CategoryEducationalDegreeDTO>, { idCategoryEducationalDegree: string }>({
      query: ({ idCategoryEducationalDegree }): any => ({
        url: `/CategoryEducationalDegree/getCategoryEducationalDegreeById`,
        params: { idCategoryEducationalDegree }
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryEducationalDegreeApisService", id: arg.idCategoryEducationalDegree }]
    }),
    InsertCategoryEducationalDegree: builder.mutation<payloadResult, { CategoryEducationalDegree: Partial<CategoryEducationalDegreeDTO> }>({
      query: ({ CategoryEducationalDegree }) => ({
        url: `/CategoryEducationalDegree/insertCategoryEducationalDegree`,
        method: "POST",
        data: CategoryEducationalDegree
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryEducationalDegreeApisService", id: "LIST" }] : [])
    }),
    DeleteCategoryEducationalDegree: builder.mutation<payloadResult, { idCategoryEducationalDegree: string[] }>({
      query: ({ idCategoryEducationalDegree }) => ({
        url: `/CategoryEducationalDegree/DeleteCategoryEducationalDegree`,
        method: "DELETE",
        data: idCategoryEducationalDegree
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryEducationalDegreeApisService", id: "LIST" }] : [])
    })
  })});
export const {
  useGetListCategoryEducationalDegreeQuery,
  useUpdateCategoryEducationalDegreeMutation,
  useGetCategoryEducationalDegreeByIdQuery,
  useInsertCategoryEducationalDegreeMutation,
  useDeleteCategoryEducationalDegreeMutation,
} = CategoryEducationalDegreeApisService;