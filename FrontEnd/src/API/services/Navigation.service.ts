import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "~/models/common";
import { Navigation, NavigationDTO } from "@models/navigationDTO";

export const NavigationApisService = createApi({
  reducerPath: "NavigationApisService",
  tagTypes: ["NavigationApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListNavigation: builder.query<ListResponse<Navigation>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/navigation/getListNavigation?pageNumber=${pageNumber}&pageSize=${pageSize}`
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "NavigationApisService" as const, id })),
            {
              type: "NavigationApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "NavigationApisService", id: "LIST" }];
      }
    }),
    GetListNavigationByToken: builder.query<ListResponse<NavigationDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/navigation/getListNavigationByToken?pageNumber=${pageNumber}&pageSize=${pageSize}`
      })
    }),
    GetNavigationById: builder.query<ListResponse<Navigation>, { id: string }>({
      query: ({ id }): any => ({
        url: `/navigation/getNavigationById?idNavigation=${id}`
      }),
      providesTags: (result, error, arg) => [{ type: "NavigationApisService", id: arg.id }]
    }),
    InsertNavigation: builder.mutation<payloadResult, { role: Partial<Navigation> }>({
      query: ({ role }) => ({
        url: `/navigation/Navigation/insertNavigation`,
        method: "POST",
        data: role
      }),
      invalidatesTags: (result) => (result ? [{ type: "NavigationApisService", id: "LIST" }] : [])
    }),
    UpdateNavigation: builder.mutation<payloadResult, { role: Partial<Navigation> }>({
      query: ({ role }) => ({
        url: `/navigation/Navigation/updateNavigation`,
        method: "PUT",
        data: role
      }),
      invalidatesTags: (result) => (result ? [{ type: "NavigationApisService", id: "LIST" }] : [])
    }),
    DeleteNavigation: builder.mutation<payloadResult, { idNavigation: string[] }>({
      query: ({ idNavigation }) => ({
        url: `/navigation/Navigation/deleteNavigation`,
        method: "DELETE",
        data: idNavigation
      }),
      invalidatesTags: (result) => (result ? [{ type: "NavigationApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListNavigationQuery,
  useGetNavigationByIdQuery,
  useInsertNavigationMutation,
  useUpdateNavigationMutation,
  useDeleteNavigationMutation,
  useGetListNavigationByTokenQuery
} = NavigationApisService;
