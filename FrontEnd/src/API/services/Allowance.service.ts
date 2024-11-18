import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { AllowanceDTO } from "@models/allowanceDTO";

export const AllowanceApisService = createApi({
  reducerPath: "AllowanceApisService",
  tagTypes: ["AllowanceApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListAllowance: builder.query<ListResponse<AllowanceDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/allowance/getListAllowance`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "AllowanceApisService" as const, id })),
            {
              type: "AllowanceApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "AllowanceApisService", id: "LIST" }];
      }
    }),
    GetAllowanceById: builder.query<ListResponse<AllowanceDTO>, { idAllowance: string }>({
      query: ({ idAllowance }): any => ({
        url: `/allowance/getAllowanceById`,
        params: { idAllowance }
      }),
      providesTags: (result, error, arg) => [{ type: "AllowanceApisService", id: arg.idAllowance }]
    }),
    InsertAllowance: builder.mutation<payloadResult, { allowance: Partial<AllowanceDTO> }>({
      query: ({ allowance }) => ({
        url: `/allowance/insertAllowance`,
        method: "POST",
        data: allowance
      }),
      invalidatesTags: (result) => (result ? [{ type: "AllowanceApisService", id: "LIST" }] : [])
    }),
    UpdateAllowance: builder.mutation<payloadResult, { allowance: Partial<AllowanceDTO> }>({
      query: ({ allowance }) => ({
        url: `/allowance/updateAllowance`,
        method: "PUT",
        data: allowance
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "AllowanceApisService", id: arg.allowance.id }] : [])
    }),
    DeleteAllowance: builder.mutation<payloadResult, { idAllowance: string[] }>({
      query: ({ idAllowance }) => ({
        url: `/allowance/deleteAllowance`,
        method: "DELETE",
        data: idAllowance
      }),
      invalidatesTags: (result) => (result ? [{ type: "AllowanceApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListAllowanceQuery,
  useGetAllowanceByIdQuery,
  useInsertAllowanceMutation,
  useUpdateAllowanceMutation,
  useDeleteAllowanceMutation
} = AllowanceApisService;
