import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { ExamDutyRegistrationDetailsDTO } from "@models/ExamDutyRegistrationDetailsDTO";

export const ExamDutyRegistrationDetailsApisService = createApi({
  reducerPath: "ExamDutyRegistrationDetailsApisService",
  tagTypes: ["ExamDutyRegistrationDetailsApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListExamDutyRegistrationDetails: builder.query<ListResponse<ExamDutyRegistrationDetailsDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/ExamDutyRegistrationDetails/getListExamDutyRegistrationDetails`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ExamDutyRegistrationDetailsApisService" as const, id })),
            {
              type: "ExamDutyRegistrationDetailsApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ExamDutyRegistrationDetailsApisService", id: "LIST" }];
      }
    }),
    GetExamDutyRegistrationDetailsById: builder.query<ListResponse<ExamDutyRegistrationDetailsDTO>, { idExamDutyRegistrationDetails: string }>({
      query: ({ idExamDutyRegistrationDetails }): any => ({
        url: `/ExamDutyRegistrationDetails/getExamDutyRegistrationDetailsById`,
        params: { idExamDutyRegistrationDetails }
      }),
      providesTags: (result, error, arg) => [{ type: "ExamDutyRegistrationDetailsApisService", id: arg.idExamDutyRegistrationDetails }]
    }),
    InsertExamDutyRegistrationDetails: builder.mutation<payloadResult, { ExamDutyRegistrationDetails: Partial<ExamDutyRegistrationDetailsDTO> }>({
      query: ({ ExamDutyRegistrationDetails }) => ({
        url: `/ExamDutyRegistrationDetails/insertExamDutyRegistrationDetails`,
        method: "POST",
        data: ExamDutyRegistrationDetails
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExamDutyRegistrationDetailsApisService", id: "LIST" }] : [])
    }),
    UpdateExamDutyRegistrationDetails: builder.mutation<payloadResult, { ExamDutyRegistrationDetails: Partial<ExamDutyRegistrationDetailsDTO> }>({
      query: ({ ExamDutyRegistrationDetails }) => ({
        url: `/ExamDutyRegistrationDetails/updateExamDutyRegistrationDetails`,
        method: "PUT",
        data: ExamDutyRegistrationDetails
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "ExamDutyRegistrationDetailsApisService", id: arg.ExamDutyRegistrationDetails.id }] : [])
    }),
    DeleteExamDutyRegistrationDetails: builder.mutation<payloadResult, { idExamDutyRegistrationDetails: string[] }>({
      query: ({ idExamDutyRegistrationDetails }) => ({
        url: `/ExamDutyRegistrationDetails/deleteExamDutyRegistrationDetails`,
        method: "DELETE",
        data: idExamDutyRegistrationDetails
      }),
      invalidatesTags: (result) => (result ? [{ type: "ExamDutyRegistrationDetailsApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListExamDutyRegistrationDetailsQuery,
  useGetExamDutyRegistrationDetailsByIdQuery,
  useInsertExamDutyRegistrationDetailsMutation,
  useUpdateExamDutyRegistrationDetailsMutation,
  useDeleteExamDutyRegistrationDetailsMutation
} = ExamDutyRegistrationDetailsApisService;