import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { ResignDTO } from "@models/resignDTO";
import { globalVariable } from "~/globalVariable";

export const ResignApisService = createApi({
  reducerPath: "ResignApisService",
  tagTypes: ["ResignApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListResign: builder.query<ListResponse<ResignDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/resign/getListResign`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ResignApisService" as const, id })),
            {
              type: "ResignApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ResignApisService", id: "LIST" }];
      }
    }),
    GetResignById: builder.query<ListResponse<ResignDTO>, { idResign: string }>({
      query: ({ idResign }): any => ({
        url: `/resign/getResignById`,
        params: { idResign }
      }),
      providesTags: (result, error, arg) => [{ type: "ResignApisService", id: arg.idResign }]
    }),
    InsertResign: builder.mutation<payloadResult, { resign: Partial<ResignDTO> }>({
      query: ({ resign }) => ({
        url: `/resign/insertResign`,
        method: "POST",
        data: resign
      }),
      invalidatesTags: (result) => (result ? [{ type: "ResignApisService", id: "LIST" }] : [])
    }),
    UpdateResign: builder.mutation<payloadResult, { resign: Partial<ResignDTO> }>({
      query: ({ resign }) => ({
        url: `/resign/updateResign`,
        method: "PUT",
        data: resign
      }),
      invalidatesTags: (result, error, arg) => (result ? [{ type: "ResignApisService", id: arg.resign.id }] : [])
    }),
    DeleteResign: builder.mutation<payloadResult, { listId: string[] }>({
      query: ({ listId }) => ({
        url: `/resign/deleteResign`,
        method: "DELETE",
        data: listId
      }),
      invalidatesTags: (result) => (result ? [{ type: "ResignApisService", id: "LIST" }] : [])
    }),
    GetListResignByRole: builder.query<ListResponse<ResignDTO>, pagination>({
      query: (pagination): any => ({
        url: `/resign/getListResignByRole`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ResignApisService" as const, id })),
            {
              type: "ResignApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ResignApisService", id: "LIST" }];
      }
    }),
    GetListResignByHistory: builder.query<ListResponse<ResignDTO>, pagination>({
      query: (pagination): any => ({
        url: `/resign/getListResignByHistory`,
        method: "GET",
        params: pagination
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "ResignApisService" as const, id })),
            {
              type: "ResignApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "ResignApisService", id: "LIST" }];
      }
    })
  })
});
export const getFileResign = (idResign: string) => {
  return `${globalVariable.urlServerApi}/api/v1/resign/getFileResign?fileNameId=${idResign}`;
};
export const {
  useGetListResignQuery,
  useGetResignByIdQuery,
  useInsertResignMutation,
  useUpdateResignMutation,
  useDeleteResignMutation,
  useGetListResignByRoleQuery,
  useGetListResignByHistoryQuery
} = ResignApisService;
