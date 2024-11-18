import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { TicketLaborEquipmentDTO } from "@models/ticketLaborEquipmentDTO";
import { TicketLaborEquipmentDetailDTO } from "@models/ticketLaborEquipmentDetailDTO";
import { globalVariable } from "~/globalVariable";

export const TicketLaborEquipmentApisService = createApi({
  reducerPath: "TicketLaborEquipmentApisService",
  tagTypes: ["TicketLaborEquipmentApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListTicketLaborEquipment: builder.query<ListResponse<TicketLaborEquipmentDTO>, pagination>({
      query: ({ pageSize, pageNumber }) => ({
        url: `/ticketLaborEquipment/getListTicketLaborEquipment`,
        method: "GET",
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "TicketLaborEquipmentApisService" as const, id })),
            {
              type: "TicketLaborEquipmentApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "TicketLaborEquipmentApisService", id: "LIST" }];
      }
    }),
    GetTicketLaborEquipmentById: builder.query<
      ListResponse<TicketLaborEquipmentDTO>,
      {
        idTicketLaborEquipment: string;
      }
    >({
      query: ({ idTicketLaborEquipment }) => ({
        url: `/ticketLaborEquipment/getTicketLaborEquipmentById`,
        method: "GET",
        params: { idTicketLaborEquipment }
      }),
      providesTags: (result, error, arg) => [
        {
          type: "TicketLaborEquipmentApisService",
          id: arg.idTicketLaborEquipment
        }
      ]
    }),
    InsertTicketLaborEquipment: builder.mutation<
      payloadResult,
      {
        ticketLaborEquipment: Partial<TicketLaborEquipmentDTO>;
      }
    >({
      query: ({ ticketLaborEquipment }) => ({
        url: `/ticketLaborEquipment/insertTicketLaborEquipment`,
        method: "POST",
        data: ticketLaborEquipment
      }),
      invalidatesTags: (result) => (result ? [{ type: "TicketLaborEquipmentApisService", id: "LIST" }] : [])
    }),
    UpdateTicketLaborEquipment: builder.mutation<
      payloadResult,
      {
        ticketLaborEquipment: Partial<TicketLaborEquipmentDTO>;
      }
    >({
      query: ({ ticketLaborEquipment }) => ({
        url: `/ticketLaborEquipment/updateTicketLaborEquipment`,
        method: "PUT",
        data: ticketLaborEquipment
      }),
      invalidatesTags: (result, error, arg) => {
        if (result) {
          return [{ type: "TicketLaborEquipmentApisService", id: arg.ticketLaborEquipment.id }];
        }
        return [];
      }
    }),
    DeleteTicketLaborEquipment: builder.mutation<payloadResult, { idTicketLaborEquipment: string[] }>({
      query: ({ idTicketLaborEquipment }) => ({
        url: `/ticketLaborEquipment/deleteTicketLaborEquipment`,
        method: "DELETE",
        data: idTicketLaborEquipment
      }),
      invalidatesTags: (result) => (result ? [{ type: "TicketLaborEquipmentApisService", id: "LIST" }] : [])
    }),
    UpdateStatusTicketLaborEquipment: builder.mutation<
      payloadResult,
      {
        id: string;
        status: number;
        hrNote?: string;
        directorNote?: string;
      }
    >({
      query: (body) => ({
        url: `/ticketLaborEquipment/updateStatusTicketLaborEquipment`,
        method: "PUT",
        data: body
      }),
      invalidatesTags: (result, error, arg) => {
        if (result) {
          return [{ type: "TicketLaborEquipmentApisService", id: arg.id }];
        }
        return [];
      }
    }),
    getListTicketLaborEquipmentByRole: builder.query<ListResponse<TicketLaborEquipmentDTO>, pagination>({
      query: ({ pageSize, pageNumber }) => ({
        url: `/ticketLaborEquipment/getListTicketLaborEquipmentByRole`,
        method: "GET",
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "TicketLaborEquipmentApisService" as const, id })),
            {
              type: "TicketLaborEquipmentApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "TicketLaborEquipmentApisService", id: "LIST" }];
      }
    }),
    updateTicketLaborEquipmentDetail: builder.mutation<
      ListResponse<TicketLaborEquipmentDTO>,
      TicketLaborEquipmentDetailDTO[]
    >({
      query: (ticketLaborEquipmentDetail) => ({
        url: `/ticketLaborEquipmentDetail/updateTicketLaborEquipmentDetail`,
        method: "PUT",
        data: ticketLaborEquipmentDetail
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              { type: "TicketLaborEquipmentApisService", id: arg?.at(1)?.idTicketLaborEquipment },
              {
                type: "TicketLaborEquipmentApisService",
                id: "LIST"
              }
            ]
          : []
    }),
    getListTicketLaborEquipmentByHistory: builder.query<ListResponse<TicketLaborEquipmentDTO>, pagination>({
      query: ({ pageSize, pageNumber }) => ({
        url: `/ticketLaborEquipment/getListTicketLaborEquipmentByHistory`,
        method: "GET",
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "TicketLaborEquipmentApisService" as const, id })),
            {
              type: "TicketLaborEquipmentApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "TicketLaborEquipmentApisService", id: "LIST" }];
      }
    })
  })
});
export const getFileTicketLaborEquipment = (idRequestToHired: string) => {
  return `${globalVariable.urlServerApi}/api/v1/ticketLaborEquipment/getFileTicketLaborEquipment?fileNameId=${idRequestToHired}`;
};

export const {
  useGetListTicketLaborEquipmentQuery,
  useGetTicketLaborEquipmentByIdQuery,
  useLazyGetTicketLaborEquipmentByIdQuery,
  useInsertTicketLaborEquipmentMutation,
  useUpdateTicketLaborEquipmentMutation,
  useDeleteTicketLaborEquipmentMutation,
  useUpdateStatusTicketLaborEquipmentMutation,
  useGetListTicketLaborEquipmentByRoleQuery,
  useUpdateTicketLaborEquipmentDetailMutation,
  useGetListTicketLaborEquipmentByHistoryQuery
} = TicketLaborEquipmentApisService;
