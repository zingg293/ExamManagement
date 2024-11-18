
import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { ExamScheduleRegistrationDTO } from "@models/ExamScheduleRegistrationDTO";

export const ExamScheduleRegistrationApisService = createApi({
  reducerPath: "ExamScheduleRegistrationApisService",
  tagTypes: ["ExamScheduleRegistrationApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListExamScheduleRegistration: builder.query<ListResponse<ExamScheduleRegistrationDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/ExamScheduleRegistration/getListExamScheduleRegistration`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ExamScheduleRegistrationApisService" as const, id })),
            {
              type: "ExamScheduleRegistrationApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ExamScheduleRegistrationApisService", id: "LIST" }];
      }
    }),
    GetExamScheduleRegistrationById: builder.query<ListResponse<ExamScheduleRegistrationDTO>, { idExamScheduleRegistration: string }>({
      query: ({ idExamScheduleRegistration }): any => ({
        url: `/ExamScheduleRegistration/getExamScheduleRegistrationById`,
        params: { idExamScheduleRegistration }
      }),
      providesTags: (result, error, arg) => [{ type: "ExamScheduleRegistrationApisService", id: arg.idExamScheduleRegistration }]
    }),
    InsertExamScheduleRegistration: builder.mutation<payloadResult, { ExamScheduleRegistration: Partial<ExamScheduleRegistrationDTO> }>({
      query: ({ ExamScheduleRegistration }) => ({
        url: `/ExamScheduleRegistration/insertExamScheduleRegistration`,
        method: "POST",
        data: ExamScheduleRegistration
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExamScheduleRegistrationApisService", id: "LIST" }] : [])
    }),
    UpdateExamScheduleRegistration: builder.mutation<payloadResult, { ExamScheduleRegistration: Partial<ExamScheduleRegistrationDTO> }>({
      query: ({ ExamScheduleRegistration }) => ({
        url: `/ExamScheduleRegistration/updateExamScheduleRegistration`,
        method: "PUT",
        data: ExamScheduleRegistration
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "ExamScheduleRegistrationApisService", id: arg.ExamScheduleRegistration.id }] : [])
    }),
    DeleteExamScheduleRegistration: builder.mutation<payloadResult, { idExamScheduleRegistration: string[] }>({
      query: ({ idExamScheduleRegistration }) => ({
        url: `/ExamScheduleRegistration/deleteExamScheduleRegistration`,
        method: "DELETE",
        data: idExamScheduleRegistration
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExamScheduleRegistrationApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListExamScheduleRegistrationQuery,
  useGetExamScheduleRegistrationByIdQuery,
  useInsertExamScheduleRegistrationMutation,
  useUpdateExamScheduleRegistrationMutation,
  useDeleteExamScheduleRegistrationMutation
} = ExamScheduleRegistrationApisService;
``