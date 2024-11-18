import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import {ListResponse, pagination, payloadResult} from "@models/common";
import {CategoryProfessionalQualificationDTO} from "@models/CategoryProfessionalQualificationDTO";

export const CategoryProfessionalQualificationApisService = createApi({
  reducerPath: "CategoryProfessionalQualificationApisService",
  tagTypes: ["CategoryProfessionalQualificationApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryProfessionalQualification: builder.query<ListResponse<CategoryProfessionalQualificationDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/CategoryProfessionalQualification/getListCategoryProfessionalQualification`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryProfessionalQualificationApisService" as const, id })),
            {
              type: "CategoryProfessionalQualificationApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryProfessionalQualificationApisService", id: "LIST" }];
      }
    }),
    UpdateCategoryProfessionalQualification: builder.mutation<payloadResult, { CategoryProfessionalQualification: Partial<CategoryProfessionalQualificationDTO> }>({
      query: ({ CategoryProfessionalQualification }) => ({
        url: `/CategoryProfessionalQualification/updateCategoryProfessionalQualification`,
        method: "PUT",
        data: CategoryProfessionalQualification
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "CategoryProfessionalQualificationApisService", id: arg.CategoryProfessionalQualification.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.CategoryProfessionalQualification && arg.CategoryProfessionalQualification.id) {
          return [{ type: "CategoryProfessionalQualificationApisService", id: arg.CategoryProfessionalQualification.id }];
        }
        return [];
      }
    }),
    GetCategoryProfessionalQualificationById: builder.query<ListResponse<CategoryProfessionalQualificationDTO>, { idCategoryProfessionalQualification: string }>({
      query: ({ idCategoryProfessionalQualification }): any => ({
        url: `/CategoryProfessionalQualification/getCategoryProfessionalQualificationById`,
        params: { idCategoryProfessionalQualification }
      }),
      providesTags: (result, error, arg) => [{ type: "CategoryProfessionalQualificationApisService", id: arg.idCategoryProfessionalQualification }]
    }),
    InsertCategoryProfessionalQualification: builder.mutation<payloadResult, { CategoryProfessionalQualification: Partial<CategoryProfessionalQualificationDTO> }>({
      query: ({ CategoryProfessionalQualification }) => ({
        url: `/CategoryProfessionalQualification/insertCategoryProfessionalQualification`,
        method: "POST",
        data: CategoryProfessionalQualification
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryProfessionalQualificationApisService", id: "LIST" }] : [])
    }),
    DeleteCategoryProfessionalQualification: builder.mutation<payloadResult, { idCategoryProfessionalQualification: string[] }>({
      query: ({ idCategoryProfessionalQualification }) => ({
        url: `/CategoryProfessionalQualification/DeleteCategoryProfessionalQualification`,
        method: "DELETE",
        data: idCategoryProfessionalQualification
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryProfessionalQualificationApisService", id: "LIST" }] : [])
    })


  })});
export const {
  useGetListCategoryProfessionalQualificationQuery,
  useUpdateCategoryProfessionalQualificationMutation,
  useGetCategoryProfessionalQualificationByIdQuery,
  useInsertCategoryProfessionalQualificationMutation,
  useDeleteCategoryProfessionalQualificationMutation,
} = CategoryProfessionalQualificationApisService;