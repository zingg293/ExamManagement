import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, payloadResult } from "~/models/common";
import { WorkflowStepDTO } from "@models/workflowStepDTO";

export const WorkflowStepApisService = createApi({
  reducerPath: "WorkflowStepApisService",
  tagTypes: ["WorkflowStepApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListWorkflowStepByIdTemplate: builder.query<
      ListResponse<WorkflowStepDTO>,
      { pageNumber: number; pageSize: number; idTemplate: string }
    >({
      query: ({ pageNumber, pageSize, idTemplate }) => ({
        url: `/workflowStep/getListWorkflowStepByIdTemplate`,
        method: "Get",
        params: { pageNumber, pageSize, idTemplate }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "WorkflowStepApisService" as const, id })),
            {
              type: "WorkflowStepApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "WorkflowStepApisService", id: "LIST" }];
      }
    }),
    CudWorkflowStep: builder.mutation<payloadResult, { workflowStep: WorkflowStepDTO }>({
      query: ({ workflowStep }) => ({
        url: `/workflowStep/cudWorkflowStep`,
        method: "POST",
        data: workflowStep
      }),
      invalidatesTags: ["WorkflowStepApisService"]
    }),
    InsertWorkflowStep: builder.mutation<payloadResult, { workflowStep: WorkflowStepDTO }>({
      query: ({ workflowStep }) => ({
        url: `/workflowStep/insertWorkflowStep`,
        method: "POST",
        data: workflowStep
      }),
      invalidatesTags: (result) => (result ? [{ type: "WorkflowStepApisService", id: "LIST" }] : [])
    }),
    UpdateWorkflowStep: builder.mutation<payloadResult, { workflowStep: WorkflowStepDTO }>({
      query: ({ workflowStep }) => ({
        url: `/workflowStep/updateWorkflowStep`,
        method: "PUT",
        data: workflowStep
      }),
      invalidatesTags: (result, error, arg) =>
        result ? [{ type: "WorkflowStepApisService", id: arg.workflowStep.id }] : []
    }),
    DeleteWorkflowStep: builder.mutation<payloadResult, { idWorkflowStep: string[] }>({
      query: ({ idWorkflowStep }) => ({
        url: `/workflowStep/deleteWorkflowStep`,
        method: "DELETE",
        data: idWorkflowStep
      }),
      invalidatesTags: (result) => (result ? [{ type: "WorkflowStepApisService", id: "LIST" }] : [])
    }),
    GetWorkflowStepById: builder.query<ListResponse<WorkflowStepDTO>, { idWorkflowStep: string }>({
      query: ({ idWorkflowStep }) => ({
        url: `/workflowStep/getWorkflowStepById`,
        method: "Get",
        params: { idWorkflowStep }
      }),
      providesTags: (result, error, arg) => [{ type: "WorkflowStepApisService", id: arg.idWorkflowStep }]
    })
  })
});
export const {
  useGetListWorkflowStepByIdTemplateQuery,
  useCudWorkflowStepMutation,
  useInsertWorkflowStepMutation,
  useUpdateWorkflowStepMutation,
  useDeleteWorkflowStepMutation,
  useGetWorkflowStepByIdQuery
} = WorkflowStepApisService;
