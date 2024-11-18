import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { EmployeeBenefitsDTO } from "@models/employeeBenefitsDTO"; // Replace this import with the correct path for EmployeeBenefitsDTO

export const EmployeeBenefitsApisService = createApi({
  reducerPath: "EmployeeBenefitsApisService",
  tagTypes: ["EmployeeBenefitsApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListEmployeeBenefits: builder.query<ListResponse<EmployeeBenefitsDTO>, pagination>({
      query: (pagination): any => ({
        url: `/employeeBenefits/getListEmployeeBenefits?pageSize=${pagination.pageSize}&pageNumber=${pagination.pageNumber}`
      }),
      providesTags: ["EmployeeBenefitsApisService"]
    }),
    GetEmployeeBenefitsById: builder.query<ListResponse<EmployeeBenefitsDTO>, { idEmployeeBenefits: string }>({
      query: ({ idEmployeeBenefits }): any => ({
        url: `/employeeBenefits/getEmployeeBenefitsById?idEmployeeBenefits=${idEmployeeBenefits}`
      }),
      providesTags: (result, error, arg) => [{ type: "EmployeeBenefitsApisService", id: arg.idEmployeeBenefits }]
    }),
    InsertEmployeeBenefits: builder.mutation<payloadResult, { employeeBenefits: Partial<EmployeeBenefitsDTO> }>({
      query: ({ employeeBenefits }) => ({
        url: `/employeeBenefits/insertEmployeeBenefits`,
        method: "POST",
        data: employeeBenefits
      }),
      invalidatesTags: (result) => (result ? [{ type: "EmployeeBenefitsApisService", id: "LIST" }] : [])
    }),
    UpdateEmployeeBenefits: builder.mutation<payloadResult, { employeeBenefits: Partial<EmployeeBenefitsDTO> }>({
      query: ({ employeeBenefits }) => ({
        url: `/employeeBenefits/updateEmployeeBenefits`,
        method: "PUT",
        data: employeeBenefits
      }),
      invalidatesTags: (result, error, arg) =>
        result
          ? [
              {
                type: "EmployeeBenefitsApisService",
                id: arg.employeeBenefits.id
              }
            ]
          : []
    }),
    DeleteEmployeeBenefits: builder.mutation<payloadResult, { idEmployeeBenefits: string[] }>({
      query: ({ idEmployeeBenefits }) => ({
        url: `/employeeBenefits/deleteEmployeeBenefits`,
        method: "DELETE",
        data: idEmployeeBenefits
      }),
      invalidatesTags: (result) => (result ? [{ type: "EmployeeBenefitsApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListEmployeeBenefitsQuery,
  useGetEmployeeBenefitsByIdQuery,
  useInsertEmployeeBenefitsMutation,
  useUpdateEmployeeBenefitsMutation,
  useDeleteEmployeeBenefitsMutation
} = EmployeeBenefitsApisService;
