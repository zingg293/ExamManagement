import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { EmployeeDTO } from "@models/employeeDTO";
import { globalVariable } from "~/globalVariable";
import { EmployeeBenefitsDTO } from "@models/employeeBenefitsDTO";
import { EmployeeAllowanceDTO } from "@models/employeeAllowanceDTO"; // Replace this import with the correct path for EmployeeDTO

export const EmployeeApisService = createApi({
  reducerPath: "EmployeeApisService",
  tagTypes: ["EmployeeApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListEmployee: builder.query<ListResponse<EmployeeDTO>, pagination>({
      query: (pagination): any => ({
        url: `/employee/getListEmployee?pageSize=${pagination.pageSize}&pageNumber=${pagination.pageNumber}`
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "EmployeeApisService" as const, id })),
            {
              type: "EmployeeApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "EmployeeApisService", id: "LIST" }];
      }
    }),
    GetEmployeeById: builder.query<ListResponse<EmployeeDTO>, { idEmployee: string }>({
      query: ({ idEmployee }): any => ({
        url: `/employee/getEmployeeById?idEmployee=${idEmployee}`
      }),
      providesTags: (result, error, arg) => [{ type: "EmployeeApisService", id: arg.idEmployee }]
    }),
    InsertEmployee: builder.mutation<payloadResult, { employee: Partial<EmployeeDTO> }>({
      query: ({ employee }) => ({
        url: `/employee/insertEmployee`,
        method: "POST",
        data: employee
      }),
      invalidatesTags: (result) => (result ? [{ type: "EmployeeApisService", id: "LIST" }] : [])
    }),
    UpdateEmployee: builder.mutation<payloadResult, { employee: Partial<EmployeeDTO> }>({
      query: ({ employee }) => ({
        url: `/employee/updateEmployee`,
        method: "PUT",
        data: employee
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "EmployeeApisService", id: arg.employee.id }] : [])
    }),
    DeleteEmployee: builder.mutation<payloadResult, { idEmployee: string[] }>({
      query: ({ idEmployee }) => ({
        url: `/employee/deleteEmployee`,
        method: "DELETE",
        data: idEmployee
      }),
      invalidatesTags: (result) => (result ? [{ type: "EmployeeApisService", id: "LIST" }] : [])
    }),
    getEmployeeAndBenefits: builder.query<ListResponse<EmployeeDTO>, { idEmployee: string }>({
      query: ({ idEmployee }): any => ({
        url: `/employee/getEmployeeAndBenefits?idEmployee=${idEmployee}`,
        method: "GET"
      }),
      providesTags: (result, error, arg) => [{ type: "EmployeeApisService", id: arg.idEmployee }]
    }),
    UpdateEmployeeAndBenefits: builder.mutation<payloadResult, { employeeBenefits: Partial<EmployeeBenefitsDTO> }>({
      query: ({ employeeBenefits }) => ({
        url: `/employeeBenefits/updateEmployeeAndBenefits`,
        method: "PUT",
        data: employeeBenefits
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              {
                type: "EmployeeApisService",
                id: arg.employeeBenefits.idEmployee
              }
            ]
          : []
    }),
    getEmployeeAndAllowance: builder.query<ListResponse<EmployeeDTO>, { idEmployee: string }>({
      query: ({ idEmployee }): any => ({
        url: `/employee/getEmployeeAndAllowance?idEmployee=${idEmployee}`,
        method: "GET"
      }),
      providesTags: (result, error, arg) => [{ type: "EmployeeApisService", id: arg.idEmployee }]
    }),
    InsertEmployeeAllowance: builder.mutation<payloadResult, { employeeAllowance: Partial<EmployeeAllowanceDTO> }>({
      query: ({ employeeAllowance }) => ({
        url: `/employeeAllowance/insertEmployeeAllowance`,
        method: "POST",
        data: employeeAllowance
      }),
      invalidatesTags: (result, erorr, arg) =>
        result
          ? [
              {
                type: "EmployeeApisService",
                id: arg.employeeAllowance.idEmployee
              }
            ]
          : []
    }),
    getEmployeeResigned: builder.query<ListResponse<EmployeeDTO>, pagination>({
      query: (pagination): any => ({
        url: `/employee/getEmployeeResigned`,
        method: "GET",
        params: pagination
      }),
      providesTags: [{ type: "EmployeeApisService", id: "getEmployeeResigned" }]
    }),
    updateTypeOfEmployee: builder.mutation<payloadResult, { idEmployee: string; idTypeOfEmployee: string }>({
      query: ({ idEmployee, idTypeOfEmployee }) => ({
        url: `/employee/updateTypeOfEmployee`,
        method: "PUT",
        params: { idEmployee, idTypeOfEmployee }
      }),
      invalidatesTags: (result) =>
        result
          ? [
              {
                type: "EmployeeApisService",
                id: "getEmployeeResigned"
              }
            ]
          : []
    }),
    getListEmployeeByCondition: builder.query<
      ListResponse<EmployeeDTO>,
      { code: string; phone: string; idUnit: string; typeOfEmployee: string; pageNumber: 0; pageSize: 0 }
    >({
      query: (condition): any => ({
        url: `/employee/getListEmployeeByCondition`,
        method: "POST",
        data: condition
      })
    })
  })
});

export const GetFIleEmployee = (idEmployee: string) => {
  return `${globalVariable.urlServerApi}/api/v1/employee/getFileImage?fileNameId=${idEmployee}`;
};
export const {
  useGetListEmployeeQuery,
  useGetEmployeeByIdQuery,
  useInsertEmployeeMutation,
  useUpdateEmployeeMutation,
  useDeleteEmployeeMutation,
  useGetEmployeeAndBenefitsQuery,
  useGetEmployeeAndAllowanceQuery,
  useUpdateEmployeeAndBenefitsMutation,
  useInsertEmployeeAllowanceMutation,
  useGetEmployeeResignedQuery,
  useUpdateTypeOfEmployeeMutation,
  useLazyGetListEmployeeByConditionQuery
} = EmployeeApisService;
