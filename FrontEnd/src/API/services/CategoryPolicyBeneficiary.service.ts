import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import {ListResponse, pagination, payloadResult} from "@models/common";
import {CategoryPolicyBeneficiaryDTO} from "@models/CategoryPolicyBeneficiaryDTO";

export const CategoryPolicyBeneficiaryApisService = createApi({
  reducerPath: "CategoryPolicyBeneficiaryApisService",
  tagTypes: ["CategoryPolicyBeneficiaryApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryPolicyBeneficiary: builder.query<ListResponse<CategoryPolicyBeneficiaryDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/categoryPolicybeneficiary/getListCategoryPolicybeneficiary`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryPolicyBeneficiaryApisService" as const, id })),
            {
              type: "CategoryPolicyBeneficiaryApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryPolicyBeneficiaryApisService", id: "LIST" }];
      }
    }),
    UpdateCategoryPolicyBeneficiary: builder.mutation<payloadResult, { CategoryPolicyBeneficiary: Partial<CategoryPolicyBeneficiaryDTO> }>({
      query: ({ CategoryPolicyBeneficiary }) => ({
        url: `/categoryPolicybeneficiary/updateCategoryPolicybeneficiary`,
        method: "PUT",
        data: CategoryPolicyBeneficiary
      }),
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.CategoryPolicyBeneficiary && arg.CategoryPolicyBeneficiary.id) {
          return [{ type: "CategoryPolicyBeneficiaryApisService", id: arg.CategoryPolicyBeneficiary.id }];
        }
        return [];
      }


    }),
    GetCategoryPolicyBeneficiaryById: builder.query<ListResponse<CategoryPolicyBeneficiaryDTO>, { idCategoryPolicybeneficiary: string }>({
      query: ({ idCategoryPolicybeneficiary }): any => ({
        url: `/categoryPolicybeneficiary/getCategoryPolicybeneficiaryById`,
        params: { idCategoryPolicybeneficiary }
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryPolicyBeneficiaryApisService", id: arg.idCategoryPolicybeneficiary }]
    }),
    InsertCategoryPolicyBeneficiary: builder.mutation<payloadResult, { CategoryPolicyBeneficiary: Partial<CategoryPolicyBeneficiaryDTO> }>({
      query: ({ CategoryPolicyBeneficiary }) => ({
        url: `/categoryPolicybeneficiary/insertCategoryPolicybeneficiary`,
        method: "POST",
        data: CategoryPolicyBeneficiary
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryPolicyBeneficiaryApisService", id: "LIST" }] : [])
    }),
    DeleteCategoryPolicyBeneficiary: builder.mutation<payloadResult, { idCategoryPolicyBeneficiary: string[] }>({
      query: ({ idCategoryPolicyBeneficiary }) => ({
        url: `/categoryPolicybeneficiary/deleteCategoryPolicybeneficiary`,
        method: "DELETE",
        data: idCategoryPolicyBeneficiary
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryPolicyBeneficiaryApisService", id: "LIST" }] : [])
    })


  })});
export const {
  useGetListCategoryPolicyBeneficiaryQuery,
  useInsertCategoryPolicyBeneficiaryMutation,
  useUpdateCategoryPolicyBeneficiaryMutation,
  useGetCategoryPolicyBeneficiaryByIdQuery,
  useDeleteCategoryPolicyBeneficiaryMutation,
} = CategoryPolicyBeneficiaryApisService;