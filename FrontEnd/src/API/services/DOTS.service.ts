import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { DOTSDTO } from "@models/DOTSDTO";

export const DOTSApisService = createApi({
  reducerPath: "DOTSApisService",
  tagTypes: ["DOTSApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListDOTS: builder.query<ListResponse<DOTSDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/DOTS/getListDOTS`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "DOTSApisService" as const, id })),
            {
              type: "DOTSApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "DOTSApisService", id: "LIST" }];
      }
    }),
    UpdateDOTS: builder.mutation<
      payloadResult,
      { DOTS: Partial<DOTSDTO> }
    >({
      query: ({ DOTS }) => ({
        url: `/DOTS/updateDOTS`,
        method: "PUT",
        data: DOTS
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "DOTSApisService", id: arg.DOTS.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.DOTS && arg.DOTS.id) {
          return [{ type: "DOTSApisService", id: arg.DOTS.id }];
        }
        return [];
      }
    }),
    GetDOTSById: builder.query<ListResponse<DOTSDTO>, { idDOTS: string }>({
      query: ({ idDOTS }): any => ({
        url: `/DOTS/getDOTSById`,
        params: { idDOTS }
      }),
      providesTags: (result, error, arg) => [{ type: "DOTSApisService", id: arg.idDOTS }]
    }),
    InsertDOTS: builder.mutation<
      payloadResult,
      { DOTS: Partial<DOTSDTO> }
    >({
      query: ({ DOTS }) => ({
        url: `/DOTS/insertDOTS`,
        method: "POST",
        data: DOTS
      }),
      invalidatesTags: (result) => (result ? [{ type: "DOTSApisService", id: "LIST" }] : [])
    }),
    DeleteDOTS: builder.mutation<payloadResult, { idDOTS: string[] }>({
      query: ({ idDOTS }) => ({
        url: `/DOTS/DeleteDOTS`,
        method: "DELETE",
        data: idDOTS
      }),
      invalidatesTags: (result) => (result ? [{ type: "DOTSApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListDOTSQuery,
  useUpdateDOTSMutation,
  useGetDOTSByIdQuery,
  useInsertDOTSMutation,
  useDeleteDOTSMutation
} = DOTSApisService;
