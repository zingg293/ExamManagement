import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { CategoryCompensationBenefitsDTO } from "@models/categoryCompensationBenefitsDTO";

export const CategoryCompensationBenefitsApisService = createApi({
  reducerPath: "CategoryCompensationBenefitsApisService",
  tagTypes: ["CategoryCompensationBenefitsApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryCompensationBenefits: builder.query<ListResponse<CategoryCompensationBenefitsDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/categoryCompensationBenefits/getListCategoryCompensationBenefits?pageNumber=${pageNumber}&pageSize=${pageSize}`
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryCompensationBenefitsApisService" as const, id: id })),
            {
              type: "CategoryCompensationBenefitsApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryCompensationBenefitsApisService", id: "LIST" }];
      }
    }),
    GetCategoryCompensationBenefitsById: builder.query<ListResponse<CategoryCompensationBenefitsDTO>, { id: string }>({
      query: ({ id }): any => ({
        url: `/categoryCompensationBenefits/getCategoryCompensationBenefitsById?idCategoryCompensationBenefits=${id}`
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryCompensationBenefitsApisService", id: arg.id }]
    }),
    InsertCategoryCompensationBenefits: builder.mutation<
      payloadResult,
      { categoryCompensationBenefits: Partial<CategoryCompensationBenefitsDTO> }
    >({
      query: ({ categoryCompensationBenefits }) => ({
        url: `/categoryCompensationBenefits/insertCategoryCompensationBenefits`,
        method: "POST",
        data: categoryCompensationBenefits
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryCompensationBenefitsApisService", id: "LIST" }] : [])
    }),
    UpdateCategoryCompensationBenefits: builder.mutation<
      payloadResult,
      { categoryCompensationBenefits: Partial<CategoryCompensationBenefitsDTO> }
    >({
      query: ({ categoryCompensationBenefits }) => ({
        url: `/categoryCompensationBenefits/updateCategoryCompensationBenefits`,
        method: "PUT",
        data: categoryCompensationBenefits
      }),
      invalidatesTags: (result, error, arg) =>
        result ? [{ type: "CategoryCompensationBenefitsApisService", id: arg.categoryCompensationBenefits.id }] : []
    }),
    DeleteCategoryCompensationBenefits: builder.mutation<payloadResult, { id: string[] }>({
      query: ({ id }) => ({
        url: `/categoryCompensationBenefits/deleteCategoryCompensationBenefits`,
        method: "DELETE",
        data: id
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryCompensationBenefitsApisService", id: "LIST" }] : [])
    })
  })
});

export const {
  useGetListCategoryCompensationBenefitsQuery,
  useGetCategoryCompensationBenefitsByIdQuery,
  useInsertCategoryCompensationBenefitsMutation,
  useUpdateCategoryCompensationBenefitsMutation,
  useDeleteCategoryCompensationBenefitsMutation
} = CategoryCompensationBenefitsApisService;
