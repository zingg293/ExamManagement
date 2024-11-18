import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { CategoryLaborEquipmentDTO } from "@models/categoryLaborEquipmentDTO";

export const CategoryLaborEquipmentApisService = createApi({
  reducerPath: "CategoryLaborEquipmentApisService",
  tagTypes: ["CategoryLaborEquipmentApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListCategoryLaborEquipment: builder.query<ListResponse<CategoryLaborEquipmentDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/categoryLaborEquipment/getListCategoryLaborEquipment`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "CategoryLaborEquipmentApisService" as const, id })),
            {
              type: "CategoryLaborEquipmentApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "CategoryLaborEquipmentApisService", id: "LIST" }];
      }
    }),
    GetCategoryLaborEquipmentById: builder.query<
      ListResponse<CategoryLaborEquipmentDTO>,
      {
        idCategoryLaborEquipment: string;
      }
    >({
      query: ({ idCategoryLaborEquipment }): any => ({
        url: `/categoryLaborEquipment/getCategoryLaborEquipmentById`,
        params: { idCategoryLaborEquipment }
      }),
      providesTags: (result, error, arg) => [
        {
          type: "CategoryLaborEquipmentApisService",
          id: arg.idCategoryLaborEquipment
        }
      ]
    }),
    InsertCategoryLaborEquipment: builder.mutation<
      payloadResult,
      {
        categoryLaborEquipment: Partial<CategoryLaborEquipmentDTO>;
      }
    >({
      query: ({ categoryLaborEquipment }) => ({
        url: `/categoryLaborEquipment/insertCategoryLaborEquipment`,
        method: "POST",
        data: categoryLaborEquipment
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryLaborEquipmentApisService", id: "LIST" }] : [])
    }),
    UpdateCategoryLaborEquipment: builder.mutation<
      payloadResult,
      {
        categoryLaborEquipment: Partial<CategoryLaborEquipmentDTO>;
      }
    >({
      query: ({ categoryLaborEquipment }) => ({
        url: `/categoryLaborEquipment/updateCategoryLaborEquipment`,
        method: "PUT",
        data: categoryLaborEquipment
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              {
                type: "CategoryLaborEquipmentApisService",
                id: arg.categoryLaborEquipment.id
              }
            ]
          : []
    }),
    DeleteCategoryLaborEquipment: builder.mutation<payloadResult, { idCategoryLaborEquipment: string[] }>({
      query: ({ idCategoryLaborEquipment }) => ({
        url: `/categoryLaborEquipment/deleteCategoryLaborEquipment`,
        method: "DELETE",
        data: idCategoryLaborEquipment
      }),
      invalidatesTags: (result) => (result ? [{ type: "CategoryLaborEquipmentApisService", id: "LIST" }] : [])
    })
  })
});

export const {
  useGetListCategoryLaborEquipmentQuery,
  useGetCategoryLaborEquipmentByIdQuery,
  useInsertCategoryLaborEquipmentMutation,
  useUpdateCategoryLaborEquipmentMutation,
  useDeleteCategoryLaborEquipmentMutation
} = CategoryLaborEquipmentApisService;
