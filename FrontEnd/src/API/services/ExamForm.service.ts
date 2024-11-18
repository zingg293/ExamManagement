import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { ExamFormDTO } from "@models/ExamFormDTO";

export const ExamFormApisService = createApi({
  reducerPath: "ExamFormApisService",
  tagTypes: ["ExamFormApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListExamForm: builder.query<ListResponse<ExamFormDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/ExamForm/getListExamForm`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ExamFormApisService" as const, id })),
            {
              type: "ExamFormApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ExamFormApisService", id: "LIST" }];
      }
    }),
    UpdateExamForm: builder.mutation<
      payloadResult,
      { ExamForm: Partial<ExamFormDTO> }
    >({
      query: ({ ExamForm }) => ({
        url: `/ExamForm/updateExamForm`,
        method: "PUT",
        data: ExamForm
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "ExamFormApisService", id: arg.ExamForm.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.ExamForm && arg.ExamForm.id) {
          return [{ type: "ExamFormApisService", id: arg.ExamForm.id }];
        }
        return [];
      }
    }),
    GetExamFormById: builder.query<ListResponse<ExamFormDTO>, { idExamForm: string }>({
      query: ({ idExamForm }): any => ({
        url: `/ExamForm/getExamFormById`,
        params: { idExamForm }
      }),
      providesTags: (result, error, arg) => [{ type: "ExamFormApisService", id: arg.idExamForm }]
    }),
    InsertExamForm: builder.mutation<
      payloadResult,
      { ExamForm: Partial<ExamFormDTO> }
    >({
      query: ({ ExamForm }) => ({
        url: `/ExamForm/insertExamForm`,
        method: "POST",
        data: ExamForm
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExamFormApisService", id: "LIST" }] : [])
    }),
    DeleteExamForm: builder.mutation<payloadResult, { idExamForm: string[] }>({
      query: ({ idExamForm }) => ({
        url: `/ExamForm/DeleteExamForm`,
        method: "DELETE",
        data: idExamForm
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExamFormApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListExamFormQuery,
  useUpdateExamFormMutation,
  useGetExamFormByIdQuery,
  useInsertExamFormMutation,
  useDeleteExamFormMutation
} = ExamFormApisService;
