import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { FacultyDTO } from "@models/FacultyDTO";

export const FacultyApisService = createApi({
  reducerPath: "FacultyApisService",
  tagTypes: ["FacultyApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListFaculty: builder.query<ListResponse<FacultyDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/Faculty/getListFaculty`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "FacultyApisService" as const, id })),
            {
              type: "FacultyApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "FacultyApisService", id: "LIST" }];
      }
    }),
    UpdateFaculty: builder.mutation<payloadResult, { Faculty: Partial<FacultyDTO> }>({
      query: ({ Faculty }) => ({
        url: `/Faculty/updateFaculty`,
        method: "PUT",
        data: Faculty
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "FacultyApisService", id: arg.Faculty.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.Faculty && arg.Faculty.id) {
          return [{ type: "FacultyApisService", id: arg.Faculty.id }];
        }
        return [];
      }
    }),
    GetFacultyById: builder.query<ListResponse<FacultyDTO>, { idFaculty: string }>({
      query: ({ idFaculty }): any => ({
        url: `/Faculty/getFacultyById`,
        params: { idFaculty }
      }),
      providesTags: (result, error, arg) => [{ type: "FacultyApisService", id: arg.idFaculty }]
    }),
    InsertFaculty: builder.mutation<payloadResult, { Faculty: Partial<FacultyDTO> }>({
      query: ({ Faculty }) => ({
        url: `/Faculty/insertFaculty`,
        method: "POST",
        data: Faculty
      }),
      invalidatesTags: (result) => (result ? [{ type: "FacultyApisService", id: "LIST" }] : [])
    }),
    DeleteFaculty: builder.mutation<payloadResult, { idFaculty: string[] }>({
      query: ({ idFaculty }) => ({
        url: `/Faculty/DeleteFaculty`,
        method: "DELETE",
        data: idFaculty
      }),
      invalidatesTags: (result) => (result ? [{ type: "FacultyApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListFacultyQuery,
  useUpdateFacultyMutation,
  useGetFacultyByIdQuery,
  useInsertFacultyMutation,
  useDeleteFacultyMutation
} = FacultyApisService;
