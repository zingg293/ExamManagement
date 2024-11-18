import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { OvertimeDTO } from "@models/overtimeDTO";

export const OvertimeApisService = createApi({
  reducerPath: "OvertimeApisService",
  tagTypes: ["OvertimeApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListOvertime: builder.query<ListResponse<OvertimeDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/overtime/getListOvertime`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "OvertimeApisService" as const, id })),
            {
              type: "OvertimeApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "OvertimeApisService", id: "LIST" }];
      }
    }),
    GetOvertimeById: builder.query<ListResponse<OvertimeDTO>, { idOvertime: string }>({
      query: ({ idOvertime }): any => ({
        url: `/overtime/getOvertimeById`,
        params: { idOvertime }
      }),
      providesTags: (result, error, arg) => [{ type: "OvertimeApisService", id: arg.idOvertime }]
    }),
    InsertOvertime: builder.mutation<payloadResult, { overtime: Partial<OvertimeDTO> }>({
      query: ({ overtime }) => ({
        url: `/overtime/insertOvertime`,
        method: "POST",
        data: overtime
      }),
      invalidatesTags: (result) => (result ? [{ type: "OvertimeApisService", id: "LIST" }] : [])
    }),
    UpdateOvertime: builder.mutation<payloadResult, { overtime: Partial<OvertimeDTO> }>({
      query: ({ overtime }) => ({
        url: `/overtime/updateOvertime`,
        method: "PUT",
        data: overtime
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "OvertimeApisService", id: arg.overtime.id }] : [])
    }),
    DeleteOvertime: builder.mutation<payloadResult, { listId: string[] }>({
      query: ({ listId }) => ({
        url: `/overtime/deleteOvertime`,
        method: "DELETE",
        data: listId
      }),
      invalidatesTags: (result) => (result ? [{ type: "OvertimeApisService", id: "LIST" }] : [])
    }),
    GetListOvertimeByRole: builder.query<ListResponse<OvertimeDTO>, pagination>({
      query: (pagination): any => ({
        url: `/overtime/getListOvertimeByRole`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "OvertimeApisService" as const, id })),
            {
              type: "OvertimeApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "OvertimeApisService", id: "LIST" }];
      }
    }),
    GetListOvertimeByHistory: builder.query<ListResponse<OvertimeDTO>, pagination>({
      query: (pagination): any => ({
        url: `/overtime/getListOvertimeByHistory`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "OvertimeApisService" as const, id })),
            {
              type: "OvertimeApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "OvertimeApisService", id: "LIST" }];
      }
    }),
    filterOverTime: builder.query<
      ListResponse<OvertimeDTO>,
      {
        idEmployee: string;
        fromDate: string;
        toDate: "string";
        pageNumber: number;
        pageSize: number;
      }
    >({
      query: (filter): any => ({
        url: `/overtime/filterOverTime`,
        method: "POST",
        data: filter
      })
    })
  })
});
export const {
  useGetListOvertimeQuery,
  useGetOvertimeByIdQuery,
  useInsertOvertimeMutation,
  useUpdateOvertimeMutation,
  useDeleteOvertimeMutation,
  useGetListOvertimeByRoleQuery,
  useGetListOvertimeByHistoryQuery,
  useLazyFilterOverTimeQuery
} = OvertimeApisService;
