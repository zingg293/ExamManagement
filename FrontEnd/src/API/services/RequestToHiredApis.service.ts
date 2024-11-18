import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { RequestToHiredDTO } from "@models/requestToHiredDTO";
import { globalVariable } from "~/globalVariable";

export const RequestToHiredApisService = createApi({
  reducerPath: "RequestToHiredApisService",
  tagTypes: ["RequestToHiredApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListRequestToHired: builder.query<ListResponse<RequestToHiredDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/requestToHired/getListRequestToHired`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "RequestToHiredApisService" as const, id })),
            {
              type: "RequestToHiredApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "RequestToHiredApisService", id: "LIST" }];
      }
    }),
    getListRequestToHireByRole: builder.query<ListResponse<RequestToHiredDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/requestToHired/getListRequestToHireByRole`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "RequestToHiredApisService" as const, id })),
            {
              type: "RequestToHiredApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "RequestToHiredApisService", id: "LIST" }];
      }
    }),
    GetRequestToHiredById: builder.query<ListResponse<RequestToHiredDTO>, { idRequestToHired: string }>({
      query: ({ idRequestToHired }): any => ({
        url: `/requestToHired/getRequestToHiredById`,
        params: { idRequestToHired }
      }),
      providesTags: (result, error, arg) => [{ type: "RequestToHiredApisService", id: arg.idRequestToHired }]
    }),
    InsertRequestToHired: builder.mutation<payloadResult, { requestToHired: Partial<RequestToHiredDTO> }>({
      query: ({ requestToHired }) => ({
        url: `/requestToHired/insertRequestToHired`,
        method: "POST",
        data: requestToHired
      }),
      invalidatesTags: (result) => (result ? [{ type: "RequestToHiredApisService", id: "LIST" }] : [])
    }),
    UpdateRequestToHired: builder.mutation<payloadResult, { requestToHired: Partial<RequestToHiredDTO> }>({
      query: ({ requestToHired }) => ({
        url: `/requestToHired/updateRequestToHired`,
        method: "PUT",
        data: requestToHired
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              {
                type: "RequestToHiredApisService",
                id: arg.requestToHired.id
              }
            ]
          : []
    }),
    DeleteRequestToHired: builder.mutation<payloadResult, { idRequestToHired: string[] }>({
      query: ({ idRequestToHired }) => ({
        url: `/requestToHired/deleteRequestToHired`,
        method: "DELETE",
        data: idRequestToHired
      }),
      invalidatesTags: (result) => (result ? [{ type: "RequestToHiredApisService", id: "LIST" }] : [])
    }),
    getListRequestToHireByHistory: builder.query<ListResponse<RequestToHiredDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/requestToHired/getListRequestToHireByHistory`,
        params: { pageSize, pageNumber },
        method: "GET"
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "RequestToHiredApisService" as const, id })),
            {
              type: "RequestToHiredApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "RequestToHiredApisService", id: "LIST" }];
      }
    }),
    filterListRequestToHired: builder.query<
      ListResponse<RequestToHiredDTO>,
      {
        idUnit: string;
        idCategoryVacancies: string;
      }
    >({
      query: (filter): any => ({
        url: `/requestToHired/filterListRequestToHired`,
        params: filter,
        method: "GET"
      })
    })
  })
});

export const getFileRequestToHired = (idRequestToHired: string) => {
  return `${globalVariable.urlServerApi}/api/v1/requestToHired/getFileRequestToHired?fileNameId=${idRequestToHired}`;
};

export const {
  useGetListRequestToHiredQuery,
  useGetListRequestToHireByRoleQuery,
  useGetRequestToHiredByIdQuery,
  useInsertRequestToHiredMutation,
  useUpdateRequestToHiredMutation,
  useDeleteRequestToHiredMutation,
  useGetListRequestToHireByHistoryQuery,
  useLazyFilterListRequestToHiredQuery
} = RequestToHiredApisService;
