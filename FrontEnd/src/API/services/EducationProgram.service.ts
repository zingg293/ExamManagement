import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { EducationProgramDTO } from "@models/EducationProgramDTO";

export const EducationProgramApisService = createApi({
  reducerPath: "EducationProgramApisService",
  tagTypes: ["EducationProgramApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListEducationProgram: builder.query<ListResponse<EducationProgramDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/EducationProgram/getListEducationProgram`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "EducationProgramApisService" as const, id })),
            {
              type: "EducationProgramApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "EducationProgramApisService", id: "LIST" }];
      }
    }),
    UpdateEducationProgram: builder.mutation<
      payloadResult,
      { EducationProgram: Partial<EducationProgramDTO> }
    >({
      query: ({ EducationProgram }) => ({
        url: `/EducationProgram/updateEducationProgram`,
        method: "PUT",
        data: EducationProgram
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "EducationProgramApisService", id: arg.EducationProgram.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.EducationProgram && arg.EducationProgram.id) {
          return [{ type: "EducationProgramApisService", id: arg.EducationProgram.id }];
        }
        return [];
      }
    }),
    GetEducationProgramById: builder.query<ListResponse<EducationProgramDTO>, { idEducationProgram: string }>({
      query: ({ idEducationProgram }): any => ({
        url: `/EducationProgram/getEducationProgramById`,
        params: { idEducationProgram }
      }),
      providesTags: (result, error, arg) => [{ type: "EducationProgramApisService", id: arg.idEducationProgram }]
    }),
    InsertEducationProgram: builder.mutation<
      payloadResult,
      { EducationProgram: Partial<EducationProgramDTO> }
    >({
      query: ({ EducationProgram }) => ({
        url: `/EducationProgram/insertEducationProgram`,
        method: "POST",
        data: EducationProgram
      }),
      invalidatesTags: (result) => (result ? [{ type: "EducationProgramApisService", id: "LIST" }] : [])
    }),
    DeleteEducationProgram: builder.mutation<payloadResult, { idEducationProgram: string[] }>({
      query: ({ idEducationProgram }) => ({
        url: `/EducationProgram/DeleteEducationProgram`,
        method: "DELETE",
        data: idEducationProgram
      }),
      invalidatesTags: (result) => (result ? [{ type: "EducationProgramApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListEducationProgramQuery,
  useUpdateEducationProgramMutation,
  useGetEducationProgramByIdQuery,
  useInsertEducationProgramMutation,
  useDeleteEducationProgramMutation
} = EducationProgramApisService;
