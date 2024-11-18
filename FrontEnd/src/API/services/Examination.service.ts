import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { ExaminationDTO } from "@models/ExaminationDTO";

export const ExaminationApisService = createApi({
  reducerPath: "ExaminationApisService",
  tagTypes: ["ExaminationApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListExamination: builder.query<ListResponse<ExaminationDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/Examination/getListExamination`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ExaminationApisService" as const, id })),
            {
              type: "ExaminationApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ExaminationApisService", id: "LIST" }];
      }
    }),
    UpdateExamination: builder.mutation<payloadResult, { Examination: Partial<ExaminationDTO> }>({
      query: ({ Examination }) => ({
        url: `/Examination/updateExamination`,
        method: "PUT",
        data: Examination
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "ExaminationApisService", id: arg.Examination.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.Examination && arg.Examination.id) {
          return [{ type: "ExaminationApisService", id: arg.Examination.id }];
        }
        return [];
      }
    }),
    GetExaminationById: builder.query<ListResponse<ExaminationDTO>, { idExamination: string }>({
      query: ({ idExamination }): any => ({
        url: `/Examination/getExaminationById`,
        params: { idExamination }
      }),
      providesTags: (result, error, arg) => [{ type: "ExaminationApisService", id: arg.idExamination }]
    }),
    InsertExamination: builder.mutation<payloadResult, { Examination: Partial<ExaminationDTO> }>({
      query: ({ Examination }) => ({
        url: `/Examination/insertExamination`,
        method: "POST",
        data: Examination
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExaminationApisService", id: "LIST" }] : [])
    }),
    DeleteExamination: builder.mutation<payloadResult, { idExamination: string[] }>({
      query: ({ idExamination }) => ({
        url: `/Examination/DeleteExamination`,
        method: "DELETE",
        data: idExamination
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExaminationApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListExaminationQuery,
  useUpdateExaminationMutation,
  useGetExaminationByIdQuery,
  useInsertExaminationMutation,
  useDeleteExaminationMutation
} = ExaminationApisService;
