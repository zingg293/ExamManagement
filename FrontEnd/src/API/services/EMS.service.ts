import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { EMSDTO } from "@models/EMSDTO";

export const EMSApisService = createApi({
  reducerPath: "EMSApisService",
  tagTypes: ["EMSApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListEMS: builder.query<ListResponse<EMSDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/EMS/getListEMS`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "EMSApisService" as const, id })),
            {
              type: "EMSApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "EMSApisService", id: "LIST" }];
      }
    }),
    UpdateEMS: builder.mutation<
      payloadResult,
      { EMS: Partial<EMSDTO> }
    >({
      query: ({ EMS }) => ({
        url: `/EMS/updateEMS`,
        method: "PUT",
        data: EMS
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "EMSApisService", id: arg.EMS.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.EMS && arg.EMS.id) {
          return [{ type: "EMSApisService", id: arg.EMS.id }];
        }
        return [];
      }
    }),
    GetEMSById: builder.query<ListResponse<EMSDTO>, { idEMS: string }>({
      query: ({ idEMS }): any => ({
        url: `/EMS/getEMSById`,
        params: { idEMS }
      }),
      providesTags: (result, error, arg) => [{ type: "EMSApisService", id: arg.idEMS }]
    }),
    InsertEMS: builder.mutation<
      payloadResult,
      { EMS: Partial<EMSDTO> }
    >({
      query: ({ EMS }) => ({
        url: `/EMS/insertEMS`,
        method: "POST",
        data: EMS
      }),
      invalidatesTags: (result) => (result ? [{ type: "EMSApisService", id: "LIST" }] : [])
    }),
    DeleteEMS: builder.mutation<payloadResult, { idEMS: string[] }>({
      query: ({ idEMS }) => ({
        url: `/EMS/DeleteEMS`,
        method: "DELETE",
        data: idEMS
      }),
      invalidatesTags: (result) => (result ? [{ type: "EMSApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListEMSQuery,
  useUpdateEMSMutation,
  useGetEMSByIdQuery,
  useInsertEMSMutation,
  useDeleteEMSMutation
} = EMSApisService;
