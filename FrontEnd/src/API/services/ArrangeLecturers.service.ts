import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { ArrangeLecturersDTO } from "@models/ArrangeLecturersDTO";

export const ArrangeLecturersApisService = createApi({
  reducerPath: "ArrangeLecturersApisService",
  tagTypes: ["ArrangeLecturersApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListArrangeLecturers: builder.query<ListResponse<ArrangeLecturersDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/ArrangeLecturers/getListArrangeLecturers`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ArrangeLecturersApisService" as const, id })),
            {
              type: "ArrangeLecturersApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ArrangeLecturersApisService", id: "LIST" }];
      }
    }),
    UpdateArrangeLecturers: builder.mutation<
      payloadResult,
      { ArrangeLecturers: Partial<ArrangeLecturersDTO> }
    >({
      query: ({ ArrangeLecturers }) => ({
        url: `/ArrangeLecturers/updateArrangeLecturers`,
        method: "PUT",
        data: ArrangeLecturers
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "ArrangeLecturersApisService", id: arg.ArrangeLecturers.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.ArrangeLecturers && arg.ArrangeLecturers.id) {
          return [{ type: "ArrangeLecturersApisService", id: arg.ArrangeLecturers.id }];
        }
        return [];
      }
    }),
    GetArrangeLecturersById: builder.query<ListResponse<ArrangeLecturersDTO>, { idArrangeLecturers: string }>({
      query: ({ idArrangeLecturers }): any => ({
        url: `/ArrangeLecturers/getArrangeLecturersById`,
        params: { idArrangeLecturers }
      }),
      providesTags: (result, error, arg) => [{ type: "ArrangeLecturersApisService", id: arg.idArrangeLecturers }]
    }),
    InsertArrangeLecturers: builder.mutation<
      payloadResult,
      { ArrangeLecturers: Partial<ArrangeLecturersDTO> }
    >({
      query: ({ ArrangeLecturers }) => ({
        url: `/ArrangeLecturers/insertArrangeLecturers`,
        method: "POST",
        data: ArrangeLecturers
      }),
      invalidatesTags: (result) => (result ? [{ type: "ArrangeLecturersApisService", id: "LIST" }] : [])
    }),
    DeleteArrangeLecturers: builder.mutation<payloadResult, { idArrangeLecturers: string[] }>({
      query: ({ idArrangeLecturers }) => ({
        url: `/ArrangeLecturers/DeleteArrangeLecturers`,
        method: "DELETE",
        data: idArrangeLecturers
      }),
      invalidatesTags: (result) => (result ? [{ type: "ArrangeLecturersApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListArrangeLecturersQuery,
  useUpdateArrangeLecturersMutation,
  useGetArrangeLecturersByIdQuery,
  useInsertArrangeLecturersMutation,
  useDeleteArrangeLecturersMutation
} = ArrangeLecturersApisService;
