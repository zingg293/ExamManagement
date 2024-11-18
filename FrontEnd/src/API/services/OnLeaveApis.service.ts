import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { OnLeaveDTO } from "@models/onLeaveDTO";
import { globalVariable } from "~/globalVariable";

export const OnLeaveApisService = createApi({
  reducerPath: "OnLeaveApisService",
  tagTypes: ["OnLeaveApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListOnLeave: builder.query<ListResponse<OnLeaveDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/onLeave/getListOnLeave`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "OnLeaveApisService" as const, id })),
            {
              type: "OnLeaveApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "OnLeaveApisService", id: "LIST" }];
      }
    }),
    GetOnLeaveById: builder.query<ListResponse<OnLeaveDTO>, { idOnLeave: string }>({
      query: ({ idOnLeave }): any => ({
        url: `/onLeave/getOnLeaveById`,
        params: { idOnLeave }
      }),
      providesTags: (result, error, arg) => [{ type: "OnLeaveApisService", id: arg.idOnLeave }]
    }),
    InsertOnLeave: builder.mutation<payloadResult, { onLeave: Partial<OnLeaveDTO> }>({
      query: ({ onLeave }) => ({
        url: `/onLeave/insertOnLeave`,
        method: "POST",
        data: onLeave
      }),
      invalidatesTags: (result) => (result ? [{ type: "OnLeaveApisService", id: "LIST" }] : [])
    }),
    UpdateOnLeave: builder.mutation<payloadResult, { onLeave: Partial<OnLeaveDTO> }>({
      query: ({ onLeave }) => ({
        url: `/onLeave/updateOnLeave`,
        method: "PUT",
        data: onLeave
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "OnLeaveApisService", id: arg.onLeave.id }] : [])
    }),
    DeleteOnLeave: builder.mutation<payloadResult, { listId: string[] }>({
      query: ({ listId }) => ({
        url: `/onLeave/deleteOnLeave`,
        method: "DELETE",
        data: listId
      }),
      invalidatesTags: (result) => (result ? [{ type: "OnLeaveApisService", id: "LIST" }] : [])
    }),
    GetListOnLeaveByRole: builder.query<ListResponse<OnLeaveDTO>, pagination>({
      query: (pagination): any => ({
        url: `/onLeave/getListOnLeaveByRole`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "OnLeaveApisService" as const, id })),
            {
              type: "OnLeaveApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "OnLeaveApisService", id: "LIST" }];
      }
    }),
    GetListOnLeaveByHistory: builder.query<ListResponse<OnLeaveDTO>, pagination>({
      query: (pagination): any => ({
        url: `/onLeave/getListOnLeaveByHistory`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "OnLeaveApisService" as const, id })),
            {
              type: "OnLeaveApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "OnLeaveApisService", id: "LIST" }];
      }
    }),
    filterListOnLeave: builder.query<
      ListResponse<OnLeaveDTO>,
      {
        idEmployee: string;
        fromDate: string;
        toDate: string;
        pageNumber: number;
        pageSize: number;
      }
    >({
      query: (filter): any => ({
        url: `/onLeave/filterListOnLeave`,
        method: "POST",
        data: filter
      })
    })
  })
});

export const getFileOnLeave = (fileNameId: string) => {
  return `${globalVariable.urlServerApi}/api/v1/onLeave/getFileOnLeave?fileNameId=${fileNameId}`;
};
export const {
  useGetListOnLeaveQuery,
  useGetOnLeaveByIdQuery,
  useInsertOnLeaveMutation,
  useUpdateOnLeaveMutation,
  useDeleteOnLeaveMutation,
  useGetListOnLeaveByRoleQuery,
  useGetListOnLeaveByHistoryQuery,
  useLazyFilterListOnLeaveQuery
} = OnLeaveApisService;
