import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { RollCallDTO } from "@models/RollCallDTO";

export const RollCallApisService = createApi({
  reducerPath: "RollCallApisService",
  tagTypes: ["RollCallApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListRollCall: builder.query<ListResponse<RollCallDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/RollCall/getListRollCall`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "RollCallApisService" as const, id })),
            {
              type: "RollCallApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "RollCallApisService", id: "LIST" }];
      }
    }),
    GetRollCallById: builder.query<ListResponse<RollCallDTO>, { idRollCall: string }>({
      query: ({ idRollCall }): any => ({
        url: `/RollCall/getRollCallById`,
        params: { idRollCall }
      }),
      providesTags: (result, error, arg) => [{ type: "RollCallApisService", id: arg.idRollCall }]
    }),
    InsertRollCall: builder.mutation<payloadResult, { RollCall: Partial<RollCallDTO> }>({
      query: ({ RollCall }) => ({
        url: `/RollCall/insertRollCall`,
        method: "POST",
        data: RollCall
      }),
      invalidatesTags: (result) => (result ? [{ type: "RollCallApisService", id: "LIST" }] : [])
    }),
    UpdateRollCall: builder.mutation<payloadResult, { RollCall: Partial<RollCallDTO> }>({
      query: ({ RollCall }) => ({
        url: `/RollCall/updateRollCall`,
        method: "PUT",
        data: RollCall
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "RollCallApisService", id: arg.RollCall.id }] : [])
    }),
    DeleteRollCall: builder.mutation<payloadResult, { idRollCall: string[] }>({
      query: ({ idRollCall }) => ({
        url: `/RollCall/deleteRollCall`,
        method: "DELETE",
        data: idRollCall
      }),
      invalidatesTags: (result) => (result ? [{ type: "RollCallApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListRollCallQuery,
  useGetRollCallByIdQuery,
  useInsertRollCallMutation,
  useUpdateRollCallMutation,
  useDeleteRollCallMutation
} = RollCallApisService;