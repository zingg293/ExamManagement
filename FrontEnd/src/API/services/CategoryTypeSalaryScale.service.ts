import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import {ListResponse, pagination, payloadResult} from "@models/common";
import { CategoryTypeSalaryScaleDTO } from "@models/CategoryTypeSalaryScaleDTO";

export const CategoryTypeSalaryScaleApisService = createApi({
  reducerPath: "CategoryTypeSalaryScaleApisService",
  tagTypes: ["CategoryTypeSalaryScaleApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryTypeSalaryScale: builder.query<ListResponse<CategoryTypeSalaryScaleDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/CategoryTypeSalaryScale/getListCategoryTypeSalaryScale`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryTypeSalaryScaleApisService" as const, id })),
            {
              type: "CategoryTypeSalaryScaleApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryTypeSalaryScaleApisService", id: "LIST" }];
      }
    }),
    UpdateCategoryTypeSalaryScale: builder.mutation<payloadResult, { CategoryTypeSalaryScale: Partial<CategoryTypeSalaryScaleDTO> }>({
      query: ({ CategoryTypeSalaryScale }) => ({
        url: `/CategoryTypeSalaryScale/updateCategoryTypeSalaryScale`,
        method: "PUT",
        data: CategoryTypeSalaryScale
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "CategoryNationalityApisService", id: arg.CategoryNationality.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.CategoryTypeSalaryScale && arg.CategoryTypeSalaryScale.id) {
          return [{ type: "CategoryTypeSalaryScaleApisService", id: arg.CategoryTypeSalaryScale.id }];
        }
        return [];
      }


    }),
    GetCategoryTypeSalaryScaleById: builder.query<ListResponse<CategoryTypeSalaryScaleDTO>, { idCategoryTypeSalaryScale: string }>({
      query: ({ idCategoryTypeSalaryScale }): any => ({
        url: `/CategoryTypeSalaryScale/getCategoryTypeSalaryScaleById`,
        params: { idCategoryTypeSalaryScale }
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryTypeSalaryScaleApisService",
        id: arg.idCategoryTypeSalaryScale }]
    }),
    InsertCategoryTypeSalaryScale: builder.mutation<payloadResult, { CategoryTypeSalaryScale: Partial<CategoryTypeSalaryScaleDTO> }>({
      query: ({ CategoryTypeSalaryScale }) => ({
        url: `/CategoryTypeSalaryScale/insertCategoryTypeSalaryScale`,
        method: "POST",
        data: CategoryTypeSalaryScale
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryTypeSalaryScaleApisService", id: "LIST" }] : [])
    }),
    DeleteCategoryTypeSalaryScale: builder.mutation<payloadResult, { idCategoryTypeSalaryScale: string[] }>({
      query: ({ idCategoryTypeSalaryScale }) => ({
        url: `/CategoryTypeSalaryScale/deleteCategoryTypeSalaryScale`,
        method: "DELETE",
        data: idCategoryTypeSalaryScale
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryTypeSalaryScaleApisService", id: "LIST" }] : [])
    })


  })});
export const {
  useGetListCategoryTypeSalaryScaleQuery,
  useUpdateCategoryTypeSalaryScaleMutation,
  useGetCategoryTypeSalaryScaleByIdQuery,
  useInsertCategoryTypeSalaryScaleMutation,
  useDeleteCategoryTypeSalaryScaleMutation,
} = CategoryTypeSalaryScaleApisService;