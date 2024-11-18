import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { BusinessTripEmployeeDTO } from "@models/businessTripEmployeeDTO";

export const BusinessTripEmployeeApisService = createApi({
  reducerPath: "BusinessTripEmployeeApisService",
  tagTypes: ["BusinessTripEmployeeApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListBusinessTripEmployee: builder.query<ListResponse<BusinessTripEmployeeDTO>, pagination>({
      query: (): any => ({
        url: `/businessTripEmployee/getListBusinessTripEmployee`
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "BusinessTripEmployeeApisService" as const, id })),
            {
              type: "BusinessTripEmployeeApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "BusinessTripEmployeeApisService", id: "LIST" }];
      }
    }),
    GetBusinessTripEmployeeById: builder.query<BusinessTripEmployeeDTO, { idBusinessTripEmployee: string }>({
      query: ({ idBusinessTripEmployee }): any => ({
        url: `/businessTripEmployee/getBusinessTripEmployeeById`,
        params: { idBusinessTripEmployee }
      }),
      providesTags: (result, error, arg) => [
        {
          type: "BusinessTripEmployeeApisService",
          id: arg.idBusinessTripEmployee
        }
      ]
    }),
    InsertBusinessTripEmployee: builder.mutation<
      payloadResult,
      {
        businessTripEmployee: Partial<BusinessTripEmployeeDTO>;
      }
    >({
      query: ({ businessTripEmployee }) => ({
        url: `/businessTripEmployee/insertBusinessTripEmployee`,
        method: "POST",
        data: businessTripEmployee
      }),
      invalidatesTags: (result) => (result ? [{ type: "BusinessTripEmployeeApisService", id: "LIST" }] : [])
    }),
    UpdateBusinessTripEmployee: builder.mutation<
      payloadResult,
      {
        businessTripEmployee: Partial<BusinessTripEmployeeDTO>;
      }
    >({
      query: ({ businessTripEmployee }) => ({
        url: `/businessTripEmployee/updateBusinessTripEmployee`,
        method: "PUT",
        data: businessTripEmployee
      }),
      invalidatesTags: (result, error, arg) =>
        result ? [{ type: "BusinessTripEmployeeApisService", id: arg.businessTripEmployee.id }] : []
    }),
    DeleteBusinessTripEmployee: builder.mutation<payloadResult, { idBusinessTripEmployee: string }>({
      query: ({ idBusinessTripEmployee }) => ({
        url: `/businessTripEmployee/deleteBusinessTripEmployee`,
        method: "DELETE",
        data: idBusinessTripEmployee
      }),
      invalidatesTags: (result) => (result ? [{ type: "BusinessTripEmployeeApisService", id: "LIST" }] : [])
    }),
    InsertBusinessTripEmployeeByList: builder.mutation<
      payloadResult,
      {
        businessTripEmployees: BusinessTripEmployeeDTO[];
      }
    >({
      query: ({ businessTripEmployees }) => ({
        url: `/businessTripEmployee/insertBusinessTripEmployeeByList`,
        method: "POST",
        data: businessTripEmployees
      }),
      invalidatesTags: (result) => (result ? [{ type: "BusinessTripEmployeeApisService", id: "LIST" }] : [])
    }),
    getListBusinessTripEmployeeByIdBusinessTrip: builder.query<
      ListResponse<BusinessTripEmployeeDTO>,
      { idBusinessTrip: string }
    >({
      query: ({ idBusinessTrip }): any => ({
        url: `/businessTripEmployee/getListBusinessTripEmployeeByIdBusinessTrip`,
        params: { idBusinessTrip },
        method: "GET"
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "BusinessTripEmployeeApisService" as const, id })),
            {
              type: "BusinessTripEmployeeApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "BusinessTripEmployeeApisService", id: "LIST" }];
      }
    })
  })
});
export const {
  useGetListBusinessTripEmployeeQuery,
  useGetBusinessTripEmployeeByIdQuery,
  useInsertBusinessTripEmployeeMutation,
  useUpdateBusinessTripEmployeeMutation,
  useDeleteBusinessTripEmployeeMutation,
  useInsertBusinessTripEmployeeByListMutation,
  useGetListBusinessTripEmployeeByIdBusinessTripQuery
} = BusinessTripEmployeeApisService;
