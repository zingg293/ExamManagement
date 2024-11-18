import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import {ListResponse, pagination, payloadResult} from "@models/common";
import { CategorySalaryLevelDTO} from "@models/CategorySalaryLevelDTO";

export const CategorySalaryLevelApisService = createApi({
  reducerPath: "CategorySalaryLevelApisService",
  tagTypes: ["CategorySalaryLevelApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategorySalaryLevel: builder.query<ListResponse<CategorySalaryLevelDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/CategorySalaryLevel/getListCategorySalaryLevel`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategorySalaryLevelApisService" as const, id })),
            {
              type: "CategorySalaryLevelApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategorySalaryLevelApisService", id: "LIST" }];
      }
    }),
    UpdateCategorySalaryLevel: builder.mutation<payloadResult, { CategorySalaryLevel: Partial<CategorySalaryLevelDTO> }>({
      query: ({ CategorySalaryLevel }) => ({
        url: `/CategorySalaryLevel/updateCategorySalaryLevel`,
        method: "PUT",
        data: CategorySalaryLevel
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "CategoryNationalityApisService", id: arg.CategoryNationality.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.CategorySalaryLevel && arg.CategorySalaryLevel.id) {
          return [{ type: "CategorySalaryLevelApisService", id: arg.CategorySalaryLevel.id }];
        }
        return [];
      }


    }),
    GetCategorySalaryLevelById: builder.query<ListResponse<CategorySalaryLevelDTO>, { idCategorySalaryLevel: string }>({
      query: ({ idCategorySalaryLevel }): any => ({
        url: `/CategorySalaryLevel/getCategorySalaryLevelById`,
        params: { idCategorySalaryLevel }
      }),
      providesTags: (result, error, arg) => [{ type: "CategorySalaryLevelApisService",
        id: arg.idCategorySalaryLevel }]
    }),
      InsertCategorySalaryLevel: builder.mutation<payloadResult, { CategorySalaryLevel: Partial<CategorySalaryLevelDTO> }>({
      query: ({ CategorySalaryLevel }) => ({
        url: `/CategorySalaryLevel/insertCategorySalaryLevel`,
        method: "POST",
        data: CategorySalaryLevel
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategorySalaryLevelApisService", id: "LIST" }] : [])
    }),
    DeleteCategorySalaryLevel: builder.mutation<payloadResult, { idCategorySalaryLevel: string[] }>({
      query: ({ idCategorySalaryLevel }) => ({
        url: `/CategorySalaryLevel/deleteCategorySalaryLevel`,
        method: "DELETE",
        data: idCategorySalaryLevel
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategorySalaryLevelApisService", id: "LIST" }] : [])
    })


  })});
export const {
  useGetListCategorySalaryLevelQuery,
  useUpdateCategorySalaryLevelMutation,
  useGetCategorySalaryLevelByIdQuery,
  useInsertCategorySalaryLevelMutation,
  useDeleteCategorySalaryLevelMutation,
} = CategorySalaryLevelApisService;