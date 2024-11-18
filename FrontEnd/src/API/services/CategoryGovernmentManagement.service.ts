import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import {ListResponse, pagination, payloadResult} from "@models/common";
import CategoryGovernmentManagementDTO from "@models/CategoryGovernmentManagementDTO";

export const CategoryGovernmentManagementApisService = createApi({
  reducerPath: "CategoryGovernmentManagementApisService",
  tagTypes: ["CategoryGovernmentManagementApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryGovernmentManagement: builder.query<ListResponse<CategoryGovernmentManagementDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/CategoryGovernmentManagement/getListCategoryGovernmentManagement`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryGovernmentManagementApisService" as const, id })),
            {
              type: "CategoryGovernmentManagementApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryGovernmentManagementApisService", id: "LIST" }];
      }
    }),
    UpdateCategoryGovernmentManagement: builder.mutation<payloadResult, { CategoryGovernmentManagement: Partial<CategoryGovernmentManagementDTO> }>({
      query: ({ CategoryGovernmentManagement }) => ({
        url: `/CategoryGovernmentManagement/updateCategoryGovernmentManagement`,
        method: "PUT",
        data: CategoryGovernmentManagement
      }),
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.CategoryGovernmentManagement && arg.CategoryGovernmentManagement.id) {
          return [{ type: "CategoryGovernmentManagementApisService", id: arg.CategoryGovernmentManagement.id }];
        }
        return [];
      }
    }),
    GetCategoryGovernmentManagementById: builder.query<ListResponse<CategoryGovernmentManagementDTO>, { idCategoryGovernmentManagement: string }>({
      query: ({ idCategoryGovernmentManagement }): any => ({
        url: `/CategoryGovernmentManagement/getCategoryGovernmentManagementById`,
        params: { idCategoryGovernmentManagement }
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryGovernmentManagementApisService", id: arg.idCategoryGovernmentManagement }]
    }),
    InsertCategoryGovernmentManagement: builder.mutation<payloadResult, { CategoryGovernmentManagement: Partial<CategoryGovernmentManagementDTO> }>({
      query: ({ CategoryGovernmentManagement }) => ({
        url: `/CategoryGovernmentManagement/insertCategoryGovernmentManagement`,
        method: "POST",
        data: CategoryGovernmentManagement
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryGovernmentManagementApisService", id: "LIST" }] : [])
    }),
    DeleteCategoryGovernmentManagement: builder.mutation<payloadResult, { idCategoryGovernmentManagement: string[] }>({
      query: ({ idCategoryGovernmentManagement }) => ({
        url: `/CategoryGovernmentManagement/deleteCategoryGovernmentManagement`,
        method: "DELETE",
        data: idCategoryGovernmentManagement
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryGovernmentManagementApisService", id: "LIST" }] : [])
    })


  })});
export const {
  useGetListCategoryGovernmentManagementQuery,
  useUpdateCategoryGovernmentManagementMutation,
  useGetCategoryGovernmentManagementByIdQuery,
  useInsertCategoryGovernmentManagementMutation,
  useDeleteCategoryGovernmentManagementMutation,
} = CategoryGovernmentManagementApisService;