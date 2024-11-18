import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { WorkflowTemplateDTO } from "@models/workflowTemplateDTO";

export const WorkflowTemplateApisService = createApi({
  reducerPath: "WorkflowTemplateApisService",
  tagTypes: ["WorkflowTemplateApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListWorkflowTemplate: builder.query<ListResponse<WorkflowTemplateDTO>, pagination>({
      query: (params) => ({
        url: `/workflowTemplate/getListWorkflowTemplate`,
        method: "GET",
        params
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "WorkflowTemplateApisService" as const, id })),
            {
              type: "WorkflowTemplateApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "WorkflowTemplateApisService", id: "LIST" }];
      }
    }),
    GetWorkflowTemplateById: builder.query<ListResponse<WorkflowTemplateDTO>, { idWorkflowTemplate: string }>({
      query: ({ idWorkflowTemplate }) => ({
        url: `/workflowTemplate/getWorkflowTemplateById`,
        method: "GET",
        params: { idWorkflowTemplate }
      }),
      providesTags: (result, error, arg) => [{ type: "WorkflowTemplateApisService", id: arg.idWorkflowTemplate }]
    }),
    UpdateWorkflowTemplate: builder.mutation<payloadResult, { workflowTemplate: Partial<WorkflowTemplateDTO> }>({
      query: ({ workflowTemplate }) => ({
        url: `/workflowTemplate/updateWorkflowTemplate`,
        method: "PUT",
        data: workflowTemplate
      }),
      invalidatesTags: (result, error, arg) =>
        result ? [{ type: "WorkflowTemplateApisService", id: arg.workflowTemplate.id }] : []
    }),
    InsertWorkflowTemplate: builder.mutation<payloadResult, { workflowTemplate: Partial<WorkflowTemplateDTO> }>({
      query: ({ workflowTemplate }) => ({
        url: `/workflowTemplate/insertWorkflowTemplate`,
        method: "POST",
        data: workflowTemplate
      }),
      invalidatesTags: (result) => (result ? [{ type: "WorkflowTemplateApisService", id: "LIST" }] : [])
    }),
    DeleteWorkflowTemplate: builder.mutation<payloadResult, { idWorkflowTemplate: string[] }>({
      query: ({ idWorkflowTemplate }) => ({
        url: `/workflowTemplate/deleteWorkflowTemplate`,
        method: "DELETE",
        data: idWorkflowTemplate
      }),
      invalidatesTags: (result) => (result ? [{ type: "WorkflowTemplateApisService", id: "LIST" }] : [])
    })
  })
});

export const {
  useGetListWorkflowTemplateQuery,
  useGetWorkflowTemplateByIdQuery,
  useUpdateWorkflowTemplateMutation,
  useInsertWorkflowTemplateMutation,
  useDeleteWorkflowTemplateMutation
} = WorkflowTemplateApisService;
