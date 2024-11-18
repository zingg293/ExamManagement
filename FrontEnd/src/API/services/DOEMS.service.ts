import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { DOEMSDTO } from "@models/DOEMSDTO";

export const DOEMSApisService = createApi({
  reducerPath: "DOEMSApisService",
  tagTypes: ["DOEMSApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListDOEMS: builder.query<ListResponse<DOEMSDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/DOEMS/getListDOEMS`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "DOEMSApisService" as const, id })),
            {
              type: "DOEMSApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "DOEMSApisService", id: "LIST" }];
      }
    }),
    UpdateDOEMS: builder.mutation<
      payloadResult,
      { DOEMS: Partial<DOEMSDTO> }
    >({
      query: ({ DOEMS }) => ({
        url: `/DOEMS/updateDOEMS`,
        method: "PUT",
        data: DOEMS
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "DOEMSApisService", id: arg.DOEMS.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.DOEMS && arg.DOEMS.id) {
          return [{ type: "DOEMSApisService", id: arg.DOEMS.id }];
        }
        return [];
      }
    }),
    GetDOEMSById: builder.query<ListResponse<DOEMSDTO>, { idDOEMS: string }>({
      query: ({ idDOEMS }): any => ({
        url: `/DOEMS/getDOEMSById`,
        params: { idDOEMS }
      }),
      providesTags: (result, error, arg) => [{ type: "DOEMSApisService", id: arg.idDOEMS }]
    }),
    InsertDOEMS: builder.mutation<
      payloadResult,
      { DOEMS: Partial<DOEMSDTO> }
    >({
      query: ({ DOEMS }) => ({
        url: `/DOEMS/insertDOEMS`,
        method: "POST",
        data: DOEMS
      }),
      invalidatesTags: (result) => (result ? [{ type: "DOEMSApisService", id: "LIST" }] : [])
    }),
    DeleteDOEMS: builder.mutation<payloadResult, { idDOEMS: string[] }>({
      query: ({ idDOEMS }) => ({
        url: `/DOEMS/DeleteDOEMS`,
        method: "DELETE",
        data: idDOEMS
      }),
      invalidatesTags: (result) => (result ? [{ type: "DOEMSApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListDOEMSQuery,
  useUpdateDOEMSMutation,
  useGetDOEMSByIdQuery,
  useInsertDOEMSMutation,
  useDeleteDOEMSMutation
} = DOEMSApisService;
