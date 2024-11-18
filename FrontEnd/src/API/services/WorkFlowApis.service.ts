import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse } from "@models/common";
import { WorkflowHistoryDTO } from "@models/workflowHistoryDTO";

export const WorkFlowApisService = createApi({
  reducerPath: "WorkFlowApisService",
  tagTypes: ["WorkFlowApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    updateStepWorkFlow: builder.mutation<
      ListResponse<any>,
      {
        idWorkFlowInstance: string;
        isTerminated: boolean;
        isRequestToChange: boolean;
        message: string;
      }
    >({
      query: (data) => ({
        url: `/workFlow/updateStepWorkFlow`,
        method: "PUT",
        data
      }),
      invalidatesTags: (result, error, arg) => [{ type: "WorkFlowApisService", id: arg.idWorkFlowInstance }]
    }),
    getListWorkflowHistoriesByIdInstance: builder.query<
      ListResponse<WorkflowHistoryDTO>,
      { idWorkFlowInstance: string }
    >({
      query: ({ idWorkFlowInstance }) => ({
        url: `/workFlow/getListWorkflowHistoriesByIdInstance`,
        method: "GET",
        params: { idWorkFlowInstance }
      }),
      providesTags: (result, error, arg) => [{ type: "WorkFlowApisService", id: arg.idWorkFlowInstance }]
    })
  })
});

export const {
  useUpdateStepWorkFlowMutation,
  useGetListWorkflowHistoriesByIdInstanceQuery,
  useLazyGetListWorkflowHistoriesByIdInstanceQuery
} = WorkFlowApisService;
