import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { LecturersDTO } from "@models/LecturersDTO";

export const LecturersApisService = createApi({
  reducerPath: "LecturersApisService",
  tagTypes: ["LecturersApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListLecturers: builder.query<ListResponse<LecturersDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/Lecturers/getListLecturers`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "LecturersApisService" as const, id })),
            {
              type: "LecturersApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "LecturersApisService", id: "LIST" }];
      }
    }),
    UpdateLecturers: builder.mutation<
      payloadResult,
      { Lecturers: Partial<LecturersDTO> }
    >({
      query: ({ Lecturers }) => ({
        url: `/Lecturers/updateLecturers`,
        method: "PUT",
        data: Lecturers
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "LecturersApisService", id: arg.Lecturers.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.Lecturers && arg.Lecturers.id) {
          return [{ type: "LecturersApisService", id: arg.Lecturers.id }];
        }
        return [];
      }
    }),
    GetLecturersById: builder.query<ListResponse<LecturersDTO>, { idLecturers: string }>({
      query: ({ idLecturers }): any => ({
        url: `/Lecturers/getLecturersById`,
        params: { idLecturers }
      }),
      providesTags: (result, error, arg) => [{ type: "LecturersApisService", id: arg.idLecturers }]
    }),
    InsertLecturers: builder.mutation<
      payloadResult,
      { Lecturers: Partial<LecturersDTO> }
    >({
      query: ({ Lecturers }) => ({
        url: `/Lecturers/insertLecturers`,
        method: "POST",
        data: Lecturers
      }),
      invalidatesTags: (result) => (result ? [{ type: "LecturersApisService", id: "LIST" }] : [])
    }),
    DeleteLecturers: builder.mutation<payloadResult, { idLecturers: string[] }>({
      query: ({ idLecturers }) => ({
        url: `/Lecturers/DeleteLecturers`,
        method: "DELETE",
        data: idLecturers
      }),
      invalidatesTags: (result) => (result ? [{ type: "LecturersApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListLecturersQuery,
  useUpdateLecturersMutation,
  useGetLecturersByIdQuery,
  useInsertLecturersMutation,
  useDeleteLecturersMutation
} = LecturersApisService;
