import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { PromotionTransferDTO } from "@models/promotionTransferDTO";

export const PromotionTransferApisService = createApi({
  reducerPath: "PromotionTransferApisService",
  tagTypes: ["PromotionTransferApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetPromotionTransferById: builder.query<ListResponse<PromotionTransferDTO>, { idPromotionTransfer: string }>({
      query: ({ idPromotionTransfer }): any => ({
        url: `/PromotionTransfer/getPromotionTransferById`,
        params: { idPromotionTransfer }
      }),
      providesTags: (result, error, arg) => [{ type: "PromotionTransferApisService", id: arg.idPromotionTransfer }]
    }),
    InsertPromotionTransfer: builder.mutation<payloadResult, { PromotionTransfer: Partial<PromotionTransferDTO> }>({
      query: ({ PromotionTransfer }) => ({
        url: `/PromotionTransfer/insertPromotionTransfer`,
        method: "POST",
        data: PromotionTransfer
      }),
      invalidatesTags: (result) => (result ? [{ type: "PromotionTransferApisService", id: "LIST" }] : [])
    }),
    UpdatePromotionTransfer: builder.mutation<payloadResult, { PromotionTransfer: Partial<PromotionTransferDTO> }>({
      query: ({ PromotionTransfer }) => ({
        url: `/PromotionTransfer/updatePromotionTransfer`,
        method: "PUT",
        data: PromotionTransfer
      }),
      invalidatesTags: (result, error, arg) =>
        result ? [{ type: "PromotionTransferApisService", id: arg.PromotionTransfer.id }] : []
    }),
    DeletePromotionTransfer: builder.mutation<payloadResult, { idPromotionTransfer: string[] }>({
      query: ({ idPromotionTransfer }) => ({
        url: `/PromotionTransfer/deletePromotionTransfer`,
        method: "DELETE",
        data: idPromotionTransfer
      }),
      invalidatesTags: (result) => (result ? [{ type: "PromotionTransferApisService", id: "LIST" }] : [])
    }),
    GetListPromotionTransferByRole: builder.query<ListResponse<PromotionTransferDTO>, pagination>({
      query: (pagination): any => ({
        url: `/PromotionTransfer/getListPromotionTransferByRole`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "PromotionTransferApisService" as const, id })),
            {
              type: "PromotionTransferApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "PromotionTransferApisService", id: "LIST" }];
      }
    }),
    GetListPromotionTransferByHistory: builder.query<ListResponse<PromotionTransferDTO>, pagination>({
      query: (pagination): any => ({
        url: `/PromotionTransfer/getListPromotionTransferByHistory`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "PromotionTransferApisService" as const, id })),
            {
              type: "PromotionTransferApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "PromotionTransferApisService", id: "LIST" }];
      }
    }),
    filterPromotionTransfer: builder.query<
      ListResponse<PromotionTransferDTO>,
      {
        pageNumber: number;
        pageSize: number;
        idUnit: string;
        idEmployee: string;
      }
    >({
      query: (filter) => ({
        url: `/PromotionTransfer/filterPromotionTransfer`,
        method: "GET",
        params: filter
      })
    })
  })
});

export const {
  useGetPromotionTransferByIdQuery,
  useInsertPromotionTransferMutation,
  useUpdatePromotionTransferMutation,
  useDeletePromotionTransferMutation,
  useGetListPromotionTransferByRoleQuery,
  useGetListPromotionTransferByHistoryQuery,
  useLazyFilterPromotionTransferQuery
} = PromotionTransferApisService;
