import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { ExamSubjectDTO } from "@models/ExamSubjectDTO";

export const ExamSubjectApisService = createApi({
  reducerPath: "ExamSubjectApisService",
  tagTypes: ["ExamSubjectApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListExamSubject: builder.query<ListResponse<ExamSubjectDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/ExamSubject/getListExamSubject`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ExamSubjectApisService" as const, id })),
            {
              type: "ExamSubjectApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ExamSubjectApisService", id: "LIST" }];
      }
    }),
    UpdateExamSubject: builder.mutation<
      payloadResult,
      { ExamSubject: Partial<ExamSubjectDTO> }
    >({
      query: ({ ExamSubject }) => ({
        url: `/ExamSubject/updateExamSubject`,
        method: "PUT",
        data: ExamSubject
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "ExamSubjectApisService", id: arg.ExamSubject.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.ExamSubject && arg.ExamSubject.id) {
          return [{ type: "ExamSubjectApisService", id: arg.ExamSubject.id }];
        }
        return [];
      }
    }),
    GetExamSubjectById: builder.query<ListResponse<ExamSubjectDTO>, { idExamSubject: string }>({
      query: ({ idExamSubject }): any => ({
        url: `/ExamSubject/getExamSubjectById`,
        params: { idExamSubject }
      }),
      providesTags: (result, error, arg) => [{ type: "ExamSubjectApisService", id: arg.idExamSubject }]
    }),
    InsertExamSubject: builder.mutation<
      payloadResult,
      { ExamSubject: Partial<ExamSubjectDTO> }
    >({
      query: ({ ExamSubject }) => ({
        url: `/ExamSubject/insertExamSubject`,
        method: "POST",
        data: ExamSubject
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExamSubjectApisService", id: "LIST" }] : [])
    }),
    DeleteExamSubject: builder.mutation<payloadResult, { idExamSubject: string[] }>({
      query: ({ idExamSubject }) => ({
        url: `/ExamSubject/DeleteExamSubject`,
        method: "DELETE",
        data: idExamSubject
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExamSubjectApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListExamSubjectQuery,
  useUpdateExamSubjectMutation,
  useGetExamSubjectByIdQuery,
  useInsertExamSubjectMutation,
  useDeleteExamSubjectMutation
} = ExamSubjectApisService;
