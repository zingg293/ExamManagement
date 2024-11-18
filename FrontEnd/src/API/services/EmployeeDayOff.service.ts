import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { EmployeeDayOffDTO } from "@models/employeeDayOffDto"; // Replace this import with the correct path for EmployeeDayOffDTO

export const EmployeeDayOffApisService = createApi({
  reducerPath: "EmployeeDayOffApisService",
  tagTypes: ["EmployeeDayOffApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListEmployeeDayOff: builder.query<ListResponse<EmployeeDayOffDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/EmployeeDayOff/getListEmployeeDayOff?pageNumber=${pageNumber}&pageSize=${pageSize}`
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "EmployeeDayOffApisService" as const, id: id })),
            {
              type: "EmployeeDayOffApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "EmployeeDayOffApisService", id: "LIST" }];
      }
    }),
    GetEmployeeDayOffById: builder.query<ListResponse<EmployeeDayOffDTO>, { id: string }>({
      query: ({ id }): any => ({
        url: `/EmployeeDayOff/getEmployeeDayOffById?idEmployeeDayOff=${id}`
      }),
      providesTags: (result, error, arg) => [{ type: "EmployeeDayOffApisService", id: arg.id }]
    }),
    InsertEmployeeDayOff: builder.mutation<payloadResult, { employeeDayOff: Partial<EmployeeDayOffDTO> }>({
      query: ({ employeeDayOff }) => ({
        url: `/EmployeeDayOff/insertEmployeeDayOff`,
        method: "POST",
        data: employeeDayOff
      }),
      invalidatesTags: (result) => (result ? [{ type: "EmployeeDayOffApisService", id: "LIST" }] : [])
    }),
    UpdateEmployeeDayOff: builder.mutation<payloadResult, { employeeDayOff: Partial<EmployeeDayOffDTO> }>({
      query: ({ employeeDayOff }) => ({
        url: `/EmployeeDayOff/updateEmployeeDayOff`,
        method: "PUT",
        data: employeeDayOff
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              {
                type: "EmployeeDayOffApisService",
                id: arg.employeeDayOff.id
              }
            ]
          : []
    }),
    DeleteEmployeeDayOff: builder.mutation<payloadResult, { id: string[] }>({
      query: ({ id }) => ({
        url: `/EmployeeDayOff/deleteEmployeeDayOff`,
        method: "DELETE",
        data: id
      }),
      invalidatesTags: (result) => (result ? [{ type: "EmployeeDayOffApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListEmployeeDayOffQuery,
  useGetEmployeeDayOffByIdQuery,
  useInsertEmployeeDayOffMutation,
  useUpdateEmployeeDayOffMutation,
  useDeleteEmployeeDayOffMutation
} = EmployeeDayOffApisService;
