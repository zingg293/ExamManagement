import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { EmployeeTypeDTO } from "@models/employeeTypeDTO";

export const EmployeeTypeApisService = createApi({
  reducerPath: "EmployeeTypeApisService",
  tagTypes: ["EmployeeTypeApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListEmployeeType: builder.query<ListResponse<EmployeeTypeDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/employeeType/getListEmployeeType?pageNumber=${pageNumber}&pageSize=${pageSize}`
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({
              type: "EmployeeTypeApisService" as const,
              id: id
            })),
            {
              type: "EmployeeTypeApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "EmployeeTypeApisService", id: "LIST" }];
      }
    }),
    GetListEmployeeTypeAvailable: builder.query<ListResponse<EmployeeTypeDTO>, pagination>({
      query: (pagination): any => ({
        url: `/employeeType/getListEmployeeTypeAvailable?pageNumber=${pagination.pageNumber}&pageSize=${pagination.pageSize}`
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({
              type: "EmployeeTypeApisService" as const,
              id: id
            })),
            {
              type: "EmployeeTypeApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "EmployeeTypeApisService", id: "LIST" }];
      }
    }),
    GetEmployeeTypeById: builder.query<ListResponse<EmployeeTypeDTO>, { id: string }>({
      query: ({ id }): any => ({
        url: `/employeeType/getEmployeeTypeById?idEmployeeType=${id}`
      }),
      providesTags: (result, error, arg) => [{ type: "EmployeeTypeApisService", id: arg.id }]
    }),
    InsertEmployeeType: builder.mutation<payloadResult, { employeeType: Partial<EmployeeTypeDTO> }>({
      query: ({ employeeType }) => ({
        url: `/employeeType/insertEmployeeType`,
        method: "POST",
        data: employeeType
      }),
      invalidatesTags: (result) => (result ? [{ type: "EmployeeTypeApisService", id: "LIST" }] : [])
    }),
    UpdateEmployeeType: builder.mutation<payloadResult, { employeeType: Partial<EmployeeTypeDTO> }>({
      query: ({ employeeType }) => ({
        url: `/employeeType/updateEmployeeType`,
        method: "PUT",
        data: employeeType
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              {
                type: "EmployeeTypeApisService",
                id: arg.employeeType.id
              }
            ]
          : []
    }),
    DeleteEmployeeType: builder.mutation<payloadResult, { id: string[] }>({
      query: ({ id }) => ({
        url: `/employeeType/deleteEmployeeType`,
        method: "DELETE",
        data: [...id]
      }),
      invalidatesTags: [
        { type: "EmployeeTypeApisService", id: "LIST" },
        {
          type: "EmployeeTypeApisService",
          id: "LIST"
        }
      ]
    }),
    HideEmployeeType: builder.mutation<payloadResult, { listId: string[]; isHide: boolean }>({
      query: (arg) => ({
        url: `/employeeType/hideEmployeeType?isHide=${arg.isHide}`,
        method: "PUT",
        data: [...arg.listId]
      }),
      invalidatesTags: [
        { type: "EmployeeTypeApisService", id: "LIST" },
        {
          type: "EmployeeTypeApisService",
          id: "LIST"
        }
      ]
    })
  })
});
export const {
  useGetListEmployeeTypeQuery,
  useGetListEmployeeTypeAvailableQuery,
  useGetEmployeeTypeByIdQuery,
  useInsertEmployeeTypeMutation,
  useUpdateEmployeeTypeMutation,
  useDeleteEmployeeTypeMutation,
  useHideEmployeeTypeMutation
} = EmployeeTypeApisService;
