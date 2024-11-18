import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { ExamShiftDTO } from "@models/ExamShiftDTO";

export const ExamShiftApisService = createApi({
  reducerPath: "ExamShiftApisService",
  tagTypes: ["ExamShiftApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListExamShift: builder.query<ListResponse<ExamShiftDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/ExamShift/getListExamShift`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ExamShiftApisService" as const, id })),
            {
              type: "ExamShiftApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ExamShiftApisService", id: "LIST" }];
      }
    }),
    GetExamShiftById: builder.query<ListResponse<ExamShiftDTO>, { idExamShift: string }>({
      query: ({ idExamShift }): any => ({
        url: `/ExamShift/getExamShiftById`,
        params: { idExamShift }
      }),
      providesTags: (result, error, arg) => [{ type: "ExamShiftApisService", id: arg.idExamShift }]
    }),
    InsertExamShift: builder.mutation<payloadResult, { ExamShift: Partial<ExamShiftDTO> }>({
      query: ({ ExamShift }) => ({
        url: `/ExamShift/insertExamShift`,
        method: "POST",
        data: ExamShift
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExamShiftApisService", id: "LIST" }] : [])
    }),
    UpdateExamShift: builder.mutation<payloadResult, { ExamShift: Partial<ExamShiftDTO> }>({
      query: ({ ExamShift }) => ({
        url: `/ExamShift/updateExamShift`,
        method: "PUT",
        data: ExamShift
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "ExamShiftApisService", id: arg.ExamShift.id }] : [])
    }),
    DeleteExamShift: builder.mutation<payloadResult, { idExamShift: string[] }>({
      query: ({ idExamShift }) => ({
        url: `/ExamShift/deleteExamShift`,
        method: "DELETE",
        data: idExamShift
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExamShiftApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListExamShiftQuery,
  useGetExamShiftByIdQuery,
  useInsertExamShiftMutation,
  useUpdateExamShiftMutation,
  useDeleteExamShiftMutation
} = ExamShiftApisService;